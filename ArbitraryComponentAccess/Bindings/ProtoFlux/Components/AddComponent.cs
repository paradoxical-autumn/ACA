using ProtoFlux.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes;

using ArbitraryComponentAccess.ProtoFlux.Components;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Components;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/ACA/Components" )]
public class AddComponent : ActionNode<FrooxEngineContext>
{
    public readonly SyncRef<INodeObjectOutput<Slot>> slot = new();
    public readonly SyncRef<INodeObjectOutput<Type>> componentType = new();
    public readonly NodeObjectOutput<Component> instantiatedComponent = new();
    public readonly SyncRef<INodeOperation> onAdded = new();
    public readonly SyncRef<INodeOperation> onFailed = new();
    
    public AddComponentLogix TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( AddComponentLogix );
    public override INode NodeInstance => TypedNodeInstance!;
    public override int NodeInputCount => base.NodeInputCount + 2;
    public override int NodeOutputCount => base.NodeOutputCount + 1;
    public override int NodeImpulseCount => base.NodeImpulseCount + 2;
    // public override string NodeName => "Add Component"; // Let's test if this is needed

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

        TypedNodeInstance = new AddComponentLogix();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is AddComponentLogix typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( AddComponentLogix ) );
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
            case 1:
                return componentType;
            default:
                index -= 2;
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
                return instantiatedComponent;
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
            return onAdded;
        case 1:
            return onFailed;
        default:
            index -= 2;
            return null!;
        }
    }
}
