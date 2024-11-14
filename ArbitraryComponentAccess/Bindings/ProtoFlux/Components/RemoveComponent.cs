using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Components;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Components;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/ACA/Components" )]
public class RemoveComponent : ActionNode<FrooxEngineContext>
{
    public readonly SyncRef<INodeObjectOutput<Component>> component = new();
    public readonly SyncRef<INodeOperation> onRemoved = new();
    public readonly SyncRef<INodeOperation> onFailed = new();
    
    public RemoveComponentLogix TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( RemoveComponentLogix );
    public override INode NodeInstance => TypedNodeInstance!;
    public override int NodeInputCount => base.NodeInputCount + 1;
    public override int NodeImpulseCount => base.NodeImpulseCount + 2;

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

        TypedNodeInstance = new RemoveComponentLogix();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is RemoveComponentLogix typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( RemoveComponentLogix ) );
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
            return onRemoved;
        case 1:
            return onFailed;
        default:
            index -= 2;
            return null!;
        }
    }
}
