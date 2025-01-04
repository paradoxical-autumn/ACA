using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Components;
using Elements.Core;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Components;

[Category("ProtoFlux/Runtimes/Execution/Nodes/ACA/Components")]
public class ComponentFromSlot : ObjectFunctionNode<FrooxEngineContext, Component>
{
    public readonly SyncRef<INodeObjectOutput<Slot>> slot = new();
    public readonly SyncRef<INodeObjectOutput<Type>> componentType = new();
    public readonly SyncRef<INodeValueOutput<int>> componentIndex = new();

    public ComponentFromSlotLogix TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof(ComponentFromSlotLogix);
    public override INode NodeInstance => TypedNodeInstance;
    public override int NodeInputCount => base.NodeInputCount + 3;

    public override void ClearInstance()
    {
        TypedNodeInstance = null!;
    }

    public override N Instantiate<N>()
    {
        if (TypedNodeInstance != null)
        {
            throw new InvalidOperationException("Node has already been instantiated");
        }

        TypedNodeInstance = new ComponentFromSlotLogix();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal(INode node)
    {
        if (node is ComponentFromSlotLogix typedNodeInstance)
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException("Node instance is not of type " + typeof(ComponentFromSlotLogix));
    }

    protected override ISyncRef GetInputInternal(ref int index)
    {
        ISyncRef inputInternal = base.GetInputInternal(ref index);
        if (inputInternal != null)
        {
            return inputInternal;
        }

        switch (index)
        {
            case 0:
                return slot;
            case 1:
                return componentType;
            case 2:
                return componentIndex;
            default:
                index -= 3;
                return null!;
        }
    }
}