using ArbitraryComponentAccess;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ProtoFlux.Core;

namespace FrooxEngine.ArbitraryComponentAccess.ProtoFlux.Components;

[Category("ProtoFlux/Runtimes/Execution/Nodes/ACA/Components")]
public class AddComponent : ActionNode<FrooxEngineContext> // change this to `AddComponentBind` and ill delete ur user root. ~ p19
{
    public readonly SyncRef<INodeObjectOutput<Slot>> slot = new();
    public readonly SyncRef<INodeObjectOutput<Type>> componentType = new();
    public readonly NodeObjectOutput<Component> instantiatedComponent = new();
    public readonly SyncRef<INodeOperation> onAdded = new();
    public readonly SyncRef<INodeOperation> onFailed = new();

    public override Type NodeType => typeof(global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.AddComponent);
    public global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.AddComponent TypedNodeInstance { get; private set; } = null!;
    public override INode NodeInstance => TypedNodeInstance;

    public override int NodeInputCount => base.NodeInputCount + 2;
    public override int NodeOutputCount => base.NodeOutputCount + 1;
    public override int NodeImpulseCount => base.NodeImpulseCount + 2;

    // @errorjan, you don't need to specify `NodeName` if ur binding is the same name. ~ p19

    public override N Instantiate<N>()
    {
        if (TypedNodeInstance != null)
        {
            throw new InvalidOperationException("Node has already been instantiated");
        }

        global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.AddComponent AddComponent2 = (TypedNodeInstance = new global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.AddComponent());
        return AddComponent2 as N;
    }

    protected override void AssociateInstanceInternal(INode node)
    {
        if (node is global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.AddComponent typedNodeInstance)
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException("Node instance is not of type " + typeof(global::ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components.AddComponent));
    }

    public override void ClearInstance()
    {
        TypedNodeInstance = null!; // i dont know what an exclamation mark means, but i like to think its me yelling at the compiler to set this to null. ~ p19
    }

    protected override ISyncRef GetInputInternal(ref int index)
    {
        ISyncRef inputInternal = base.GetInputInternal(ref index);
        // Return inputInternal if not null.
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
            default:
                index -= 2;
                return null!;
        }
    }

    protected override INodeOutput GetOutputInternal(ref int index)
    {
        INodeOutput outputInternal = base.GetOutputInternal(ref index);
        if (outputInternal != null)
        {
            return outputInternal;
        }

        switch (index)
        {
            case 0:
                return instantiatedComponent;
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
        switch (index)
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

    // if i ripped this code straight from https://github.com/ErrorJan/ResonitePlugin-EngineShennanigans/blob/master/EngineShennanigans/Components/ProtoFluxBinds/AddComponent.cs
    // id uncomment this BS that FEW automatically adds. i need you to realise that FEW is on our side.
    // i get it, i get it. it adds code at RUNTIME (fucking what??) but still.
    // it helps.
    // ~ p19

    //protected override void InitializeSyncMembers()
    //{
    //    base.InitializeSyncMembers();
    //}

    //public override ISyncMember GetSyncMember(int index)
    //{
    //    UniLogPlugin.Log("1 crash here?");
    //    return index switch
    //    {
    //        0 => persistent,
    //        1 => updateOrder,
    //        2 => EnabledField,
    //        3 => slot,
    //        4 => componentType,
    //        5 => instantiatedComponent,
    //        6 => onAdded,
    //        7 => onFailed,
    //        _ => throw new ArgumentOutOfRangeException(),
    //    };
    //}

    //public static AddComponent __New()
    //{
    //    return new AddComponent();
    //}
}