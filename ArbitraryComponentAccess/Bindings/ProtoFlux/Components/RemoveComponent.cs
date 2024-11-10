using ArbitraryComponentAccess;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ProtoFlux.Core;
using System.ComponentModel;

namespace FrooxEngine.ArbitraryComponentAccess.ProtoFlux.Components;

[Category("ProtoFlux/Runtimes/Execution/Nodes/ACA/Components")]
public class RemoveComponent : ActionNode<FrooxEngineContext>
{
    public readonly SyncRef<INodeObjectOutput<Component>> component = new();
    public readonly SyncRef<INodeOperation> onRemoved = new();
    public readonly SyncRef<INodeOperation> onFailed = new();

    public override Type NodeType => typeof(global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.RemoveComponent);
    public global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.RemoveComponent TypedNodeInstance { get; private set; } = null!;
    public override INode NodeInstance => TypedNodeInstance;

    public override int NodeInputCount => base.NodeInputCount + 1;
    public override int NodeImpulseCount => base.NodeImpulseCount + 2;

    // @errorjan, you don't need to specify `NodeName` if ur binding is the same name. ~ p19

    public override N Instantiate<N>()
    {
        if (TypedNodeInstance != null)
        {
            throw new InvalidOperationException("Node has already been instantiated");
        }

        global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.RemoveComponent RemoveComponent2 = (TypedNodeInstance = new global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.RemoveComponent());
        return RemoveComponent2 as N;
    }

    protected override void AssociateInstanceInternal(INode node)
    {
        if (node is global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.RemoveComponent typedNodeInstance)
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException("Node instance is not of type " + typeof(global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.RemoveComponent));
    }

    public override void ClearInstance()
    {
        TypedNodeInstance = null!; // i dont know what an exclamation mark means, but i like to think its me yelling at the compiler to set this to null. ~ p19
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
                return component;
            default:
                index -= 1;
                return null!;
        }
    }

    protected override ISyncRef GetImpulseInternal(ref int index)
    {
        ISyncRef impulseInternal = base.GetImpulseInternal(ref index);
        if (impulseInternal != null)
        {
            return impulseInternal;
        }

        switch(index)
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