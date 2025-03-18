/////////////////////////////////////
// ErrorJan's FluxBindingsGen      //
// MIT License                     //
/////////////////////////////////////

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ErrorsSourceGen;

[ Generator( LanguageNames.CSharp ) ]
public class FluxBindings : IIncrementalGenerator
{
    // #pragma warning disable RS1035
    // // Fuck you, I need to LOG SHIT FOR DEBUGGING PLEASE
    // private static readonly string logFilePath = System.IO.Path.Combine(
    //     System.IO.Directory.GetCurrentDirectory(),
    //     "generator_debug_log.txt");

    // private static void LogMessage(string message)
    // {
    //     try
    //     {
    //         // Append the message to the log file
    //         using (var writer = new System.IO.StreamWriter(logFilePath, true))
    //         {
    //             writer.AutoFlush = true;
    //             writer.WriteLine($"{DateTime.Now}: {message}");
    //             writer.Flush();
    //         }
    //     }
    //     catch (Exception) {}
    // }
    // private static void ClearLog()
    // {
    //     try
    //     {
    //         // Append the message to the log file
    //         using (var writer = new System.IO.StreamWriter(logFilePath, false))
    //         {
    //             writer.AutoFlush = true;
    //             writer.WriteLine($"{DateTime.Now}: Cleared!");
    //             writer.Flush();
    //         }
    //     }
    //     catch (Exception) {}
    // }
    // private static void PrintList( System.Collections.IEnumerable objs )
    // {
    //     foreach ( var obj in objs )
    //         LogMessage( obj.ToString() );
    // }
    // #pragma warning restore RS1035

    public void Initialize( IncrementalGeneratorInitializationContext context )
    {
        IncrementalValuesProvider<GeneratorSyntaxContext> allClasses = context.SyntaxProvider.CreateSyntaxProvider( 
            ( syntaxNode, _ ) => syntaxNode is ClassDeclarationSyntax node,
            ( generatorSyntaxContext, _  ) => generatorSyntaxContext
        );

        IncrementalValuesProvider<GeneratorSyntaxContext> allProtoFluxImplementationClasses = allClasses.Where( IsProtoFluxImplementClass );

        context.RegisterSourceOutput( allProtoFluxImplementationClasses, GenSource );
    }

    private string GetFormattedTypeName( INamedTypeSymbol typeSymbol )
    {
        string typeName = typeSymbol.Name;

        int a = typeName.IndexOf( "Logix" );
        if ( a != -1 )
            typeName = typeName.Remove( a );

        if ( typeSymbol.IsGenericType )
        {
            // Construct the type name with generic parameters
            string genericArgs = string.Join( ", ", typeSymbol.TypeParameters.Select( tp => tp.Name ) );
            return $"{ typeName }<{ genericArgs }>";
        }
        else
        {
            return typeName;
        }
    }


    private void GenSource( SourceProductionContext context, GeneratorSyntaxContext nodeMeta )
    {
        INamedTypeSymbol classSymbol = (nodeMeta.SemanticModel.GetDeclaredSymbol( nodeMeta.Node ) as INamedTypeSymbol)!;
        string bindingNamespace = $"FluxBinding.{classSymbol.ContainingNamespace}";
        string nodeCategory = "ACA/Uncategorized";
        string className = GetFormattedTypeName( classSymbol );
        string genericArgs = string.Join( ", ", classSymbol.BaseType?.TypeArguments.Select( 
            arg => arg.ToString().Contains( "Context" ) ? "global::" + arg.ToString() : arg.ToString() ) );
        string constraints = string.Join( " ", classSymbol.TypeParameters
            .Where( tp => 
                tp.ConstraintTypes.Length > 0 || 
                tp.HasConstructorConstraint || 
                tp.HasUnmanagedTypeConstraint || 
                tp.HasValueTypeConstraint || 
                tp.HasReferenceTypeConstraint || 
                tp.HasNotNullConstraint
            )
            .Select( tp => $"where { tp.Name } : { string.Join( ", ", 
                new string?[] { 
                    tp.HasUnmanagedTypeConstraint ? "unmanaged" : null,
                    tp.HasValueTypeConstraint ? "struct" : null,
                    tp.HasReferenceTypeConstraint ? (tp.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated ? "class?" : "class") : null, //why..
                    tp.HasNotNullConstraint ? "notnull" : null,
                }.Where( (s) => s is not null )
                .Concat( tp.ConstraintTypes.Select( c => c.ToString() ) )
                .Concat( [ tp.HasConstructorConstraint ? "new()" : null ] ).Where( (s) => s is not null )
                ) }" ).ToList() 
            );
        string fullClassName = classSymbol.ToString();
        string inputSyncRefsDecl = "\n";
        string inputSyncRefsGets = "";
        int inputSyncRefAmount = 0;
        string outputRefsDecl = "";
        string outputRefsGets = "";
        int outputRefAmount = 0;
        string callSyncRefsDecl = "";
        string callSyncRefsGets = "";
        int callSyncRefAmount = 0;

        foreach ( var att in classSymbol.GetAttributes() )
        {
            try
            {
                // If exception, then most likely this died.. Make sure to use NodeCategory and not Category!!
                var arg = att.ConstructorArguments.FirstOrDefault().Value;
                if ( att.AttributeClass?.Name == "NodeCategoryAttribute" && arg != null )
                {
                    nodeCategory = arg.ToString();
                    break;
                }
            }
            catch( Exception ) 
            {
                return;
            }
        }

        var fields = classSymbol.GetMembers().OfType<IFieldSymbol>();
        foreach ( IFieldSymbol field in fields )
        {
            if ( field.IsStatic || field.DeclaredAccessibility != Accessibility.Public )
                continue;
            
            if ( field.Type is INamedTypeSymbol fieldType2 )
            {
                string constructedField = "    public readonly {0} {1} = new();";
                string constructedType;
                string typeArgs = string.Join( ", ", fieldType2.TypeArguments );

                switch( field.Type.Name )
                {
                    // ---- Inputs ----
                    case "ObjectInput":
                        string nodeOutputType = "INodeObjectOutput";    
                        goto ContinueInput;
                    case "ValueInput":
                        nodeOutputType = "INodeValueOutput";
                    
                    ContinueInput:
                        constructedType = $"SyncRef<{ nodeOutputType }<{ typeArgs }>>";
                        string fieldDecl = string.Format( constructedField, constructedType, field.Name );
                        inputSyncRefsDecl += fieldDecl + "\n";
                        inputSyncRefsGets += $"        case {inputSyncRefAmount++}: return {field.Name};\n";
                        break;

                    // ---- Outputs ----
                    case "ObjectOutput":
                        nodeOutputType = "NodeObjectOutput";
                        goto ContinueOutput;
                    case "ValueOutput":
                        nodeOutputType = "NodeValueOutput";

                    ContinueOutput:
                        constructedType = $"{ nodeOutputType }<{ typeArgs }>";
                        fieldDecl = string.Format( constructedField, constructedType, field.Name );
                        outputRefsDecl += fieldDecl + "\n";
                        outputRefsGets += $"        case {outputRefAmount++}: return {field.Name};\n";
                        break;

                    // ---- Calls ----
                    case "Call":
                        nodeOutputType = "ISyncNodeOperation";
                        goto ContinueCalls;
                    case "Continuation":
                    case "AsyncCall":
                        nodeOutputType = "INodeOperation";

                    ContinueCalls:
                        constructedType = $"SyncRef<{ nodeOutputType }>";
                        fieldDecl = string.Format( constructedField, constructedType, field.Name );
                        callSyncRefsDecl += fieldDecl + "\n";
                        callSyncRefsGets += $"        case {callSyncRefAmount++}: return {field.Name};\n";
                        break;

                    default:
                        continue;
                }
            }
            else
                return;
        }

        string templateCode = 
$$"""
using ProtoFlux.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes;

namespace {{ bindingNamespace }};

[Category( "ProtoFlux/Runtimes/Execution/Nodes/{{ nodeCategory }}" )]
public class {{className}} : {{classSymbol.BaseType!.Name}}<{{genericArgs}}> {{constraints}}
{
    public global::{{fullClassName}} TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( global::{{fullClassName}} );
    public override INode NodeInstance => TypedNodeInstance!;

{{inputSyncRefsDecl.Remove(inputSyncRefsDecl.Length - 1)}}
    public override int NodeInputCount => base.NodeInputCount + {{inputSyncRefAmount}};
{{outputRefsDecl}}
    public override int NodeOutputCount => base.NodeOutputCount + {{outputRefAmount}};
{{callSyncRefsDecl}}
    public override int NodeImpulseCount => base.NodeImpulseCount + {{callSyncRefAmount}};

    public override void ClearInstance()
    {
        TypedNodeInstance = null!;
    }

    public override N Instantiate<N>()
    {
        if ( TypedNodeInstance != null )
        {
            throw new InvalidOperationException( "Node has already been instantiated" );
        }

        TypedNodeInstance = new global::{{fullClassName}}();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is global::{{fullClassName}} typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( global::{{fullClassName}} ) );
    }

    protected override ISyncRef GetInputInternal( ref int __index__ )
    {
        ISyncRef __inputInternal__ = base.GetInputInternal( ref __index__ );
        if ( __inputInternal__ != null )
        {
            return __inputInternal__;
        }

        switch ( __index__ )
        {
{{inputSyncRefsGets}}
        default:
            __index__ -= {{inputSyncRefAmount}};
            return null!;
        }
    }

    protected override INodeOutput GetOutputInternal( ref int __index__ )
    {
        INodeOutput __outputInternal__ = base.GetOutputInternal( ref __index__ );
        if ( __outputInternal__ != null )
        {
            return __outputInternal__;
        }

        switch ( __index__ )
        {
{{outputRefsGets}}
        default:
            __index__ -= {{outputRefAmount}};
            return null!;
        }
    }

    protected override ISyncRef GetImpulseInternal( ref int __index__ )
    {
        ISyncRef __impulseInternal__ = base.GetImpulseInternal( ref __index__ );
        if ( __impulseInternal__ != null )
        {
            return __impulseInternal__;
        }
        switch ( __index__ )
        {
{{callSyncRefsGets}}
        default:
            __index__ -= {{callSyncRefAmount}};
            return null!;
        }
    }
}
""";

        string classSymbolText = classSymbol.ToString().Replace('<', '[').Replace('>', ']');
        context.AddSource( $"FluxBinding.{classSymbolText}.g.cs", templateCode );
    }

    private bool IsProtoFluxImplementClass( GeneratorSyntaxContext nodeMeta )
    {
        string firstLine = nodeMeta.Node.SyntaxTree.GetText().Lines.First().ToString().ToLower();
        if ( firstLine.Contains( "no auto" ) || firstLine.Contains( "noauto" ) )
            return false;

        // Since our Node is of type ClassDeclarationSyntax, GetDeclaredSymbol will give us INamedTypeSymbol
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpextensions.getdeclaredsymbol?view=roslyn-dotnet-4.9.0#microsoft-codeanalysis-csharp-csharpextensions-getdeclaredsymbol(microsoft-codeanalysis-semanticmodel-microsoft-codeanalysis-csharp-syntax-basetypedeclarationsyntax-system-threading-cancellationtoken)
        INamedTypeSymbol classSymbol = (nodeMeta.SemanticModel.GetDeclaredSymbol( nodeMeta.Node ) as INamedTypeSymbol)!;

        // Iterate through the interfaces and print their fully qualified names
        foreach ( INamedTypeSymbol i in classSymbol.AllInterfaces )
        {
            if ( i.Name == "INode" && i.ContainingNamespace.ToString() == "ProtoFlux.Core" )
                return true;
        }

        // Traverse base types to check inheritance
        // INamedTypeSymbol? baseType = classSymbol.BaseType;
        // while ( baseType != null )
        // {
        //     if ( baseType.Name == "Node" && baseType.ContainingNamespace.ToString() == "ProtoFlux.Core" )
        //         return true;

        //     baseType = baseType.BaseType;
        // }

        return false;
    }
}