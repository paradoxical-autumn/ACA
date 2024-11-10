using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Fields;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Fields;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/ACA/Fields" )]
public class FieldToValue<T> : ValueFunctionNode<FrooxEngineContext, T> where T : unmanaged
{
    public readonly SyncRef<INodeObjectOutput<IField>> field = new();
    
    public FieldToValueLogix<T> TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( FieldToValueLogix<T> );
    public override INode NodeInstance => TypedNodeInstance!;
    public override int NodeInputCount => base.NodeInputCount + 1;

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

        TypedNodeInstance = new FieldToValueLogix<T>();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is FieldToValueLogix<T> typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( FieldToValueLogix<T> ) );
    }

    protected override ISyncRef GetInputInternal( ref int index )
    {
        ISyncRef inputInternal = base.GetInputInternal( ref index );
        if ( inputInternal != null )
        {
            return inputInternal;
        }

        switch ( index )
        {
            case 0:
                return field;
            default:
                index -= 1;
                return null!;
        }
    }

    // FrooxEngine Weaver Stuff:
    protected override void InitializeSyncMembers()
    {
        base.InitializeSyncMembers();
    }

    public override ISyncMember GetSyncMember(int index)
    {
        return index switch
        {
            0 => persistent, 
            1 => updateOrder, 
            2 => EnabledField,
            3 => field,
            _ => throw new ArgumentOutOfRangeException(), 
        };
    }

    public static FieldToValue<T> __New()
    {
        return new FieldToValue<T>();
    }
}
