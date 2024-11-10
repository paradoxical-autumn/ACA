// no auto
// disbles binding generation.

// shameless theft from https://github.com/ErrorJan/ResonitePlugin-EngineShennanigans/blob/master/EngineShennanigans/ProtoFlux/AddComponent.cs
// hehe my plugin won the coin flip!!

using ArbitraryComponentAccess;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components;

[Category("ACA/Components")]
public class AddComponent : ActionNode<FrooxEngineContext>
{
    public ObjectInput<Slot> slot;
    public ObjectInput<Type> componentType;
    public readonly ObjectOutput<Component> instantiatedComponent;
    public Continuation onAdded;
    public Continuation onFailed;

    public AddComponent()
    {
        instantiatedComponent = new(this);
    }

    protected override IOperation Run(FrooxEngineContext context)
    {
        Slot? s = slot!.Evaluate(context);
        Type? t = componentType!.Evaluate(context);
        if (s == null || t == null)
            return onFailed.Target;

        if (!t.IsSubclassOf(typeof(Component)) || t.GetConstructor(Type.EmptyTypes) == null || t.ContainsGenericParameters)
            return onFailed.Target;

        Component ic;
        try
        {
            ic = s.AttachComponent(t);
        }
        catch (Exception)
        {
            UniLogPlugin.Log("AttachComponent on AddComponent failed???", true);
            return onFailed.Target;
        }

        instantiatedComponent.Write(ic, context);

        return onAdded.Target;
    }
}