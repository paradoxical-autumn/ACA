// no auto
// disbles binding generation.

// shameless theft from https://github.com/ErrorJan/ResonitePlugin-EngineShennanigans/blob/master/EngineShennanigans/Components/ProtoFluxBinds/RemoveComponent.cs
// hehe my plugin won the coin flip!!

using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[Category("ACA/Components")]
public class RemoveComponentLogix : ActionNode<FrooxEngineContext>
{
    public ObjectInput<Component> component;

    public Continuation onRemoved;
    public Continuation onFailed;

    protected override IOperation Run(FrooxEngineContext context)
    {
        Component? c = component!.Evaluate(context);

        if (c == null) 
            return onFailed.Target;

        // c.Destroy() is safer as it actually calls OnDestroying(), letting the component clean up after itself.
        c.Destroy();

        return onRemoved.Target;
    }
}