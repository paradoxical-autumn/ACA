// no auto
// disbles binding generation.

// shameless theft from https://github.com/ErrorJan/ResonitePlugin-EngineShennanigans/blob/master/EngineShennanigans/ProtoFlux/AddComponent.cs
// hehe my plugin won the coin flip!!

using FrooxEngine;
using ProtoFlux.Core;

using ArbitraryComponentAccess.Components;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class AddComponentLogix : ActionNode<FrooxEngineContext>
{
    public ObjectInput<Slot> slot;
    public ObjectInput<Type> componentType;
    public readonly ObjectOutput<Component> instantiatedComponent;
    public Continuation onAdded;
    public Continuation onFailed;

    public AddComponentLogix()
    {
        instantiatedComponent = new(this);
    }

    protected override IOperation Run(FrooxEngineContext context)
    {
        Slot? s = slot!.Evaluate(context);
        Type? t = componentType!.Evaluate(context);
        if (s == null || t == null)
            return onFailed.Target;

        if(!context.World.Permissions.CheckAll((ACAPermissions p) => p.is_ACA_Allowed))
            return onFailed.Target;

        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.IsTypeAllowedForExecution(t)))
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