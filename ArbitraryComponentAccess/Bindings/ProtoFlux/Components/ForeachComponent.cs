using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Components;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Components;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/ACA/Components" )]
public class ForeachComponent : ActionNode<FrooxEngineContext>
{
    public readonly SyncRef<INodeObjectOutput<Slot>> slot = new();
    public readonly NodeObjectOutput<Component> component = new();
    public readonly SyncRef<ISyncNodeOperation> loopStart = new();
    public readonly SyncRef<ISyncNodeOperation> loopIteration = new();
    public readonly SyncRef<INodeOperation> loopEnd = new();
    
    public ForeachComponentLogix TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( ForeachComponentLogix );
    public override INode NodeInstance => TypedNodeInstance!;
    public override int NodeInputCount => base.NodeInputCount + 1;
    public override int NodeOutputCount => base.NodeOutputCount + 1;
    public override int NodeImpulseCount => base.NodeImpulseCount + 3;

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

        TypedNodeInstance = new ForeachComponentLogix();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is ForeachComponentLogix typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( ForeachComponentLogix ) );
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
                return slot;
            default:
                index -= 1;
                return null!;
        }
    }

    protected override INodeOutput GetOutputInternal( ref int index )
    {
        INodeOutput outputInternal = base.GetOutputInternal( ref index );
        if ( outputInternal != null )
        {
            return outputInternal;
        }

        switch ( index )
        {
            case 0:
                return component;
            default:
                index -= 1;
                return null!;
        }
    }

    protected override ISyncRef GetImpulseInternal( ref int index )
    {
        ISyncRef impulseInternal = base.GetImpulseInternal( ref index );
        if ( impulseInternal != null )
        {
            return impulseInternal;
        }
        switch ( index )
        {
        case 0:
            return loopStart;
        case 1:
            return loopIteration;
        case 2:
            return loopEnd;
        default:
            index -= 3;
            return null!;
        }
    }
}
