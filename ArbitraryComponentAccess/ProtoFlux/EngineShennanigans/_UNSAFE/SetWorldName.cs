using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux;

[NodeCategory("EngineShennanigans")]
public class SetWorldNameLogix : ActionNode<FrooxEngineContext>
{
    public ObjectInput<string> newName;

    public Continuation onSuccess;
    public Continuation onFailure;

    protected override IOperation Run(FrooxEngineContext context)
    {
        string worldName = newName!.Evaluate(context)!;

        if (worldName == null)
        {
            return onFailure.Target;
        }

        if (!context.World.IsAuthority)
        {
            return onFailure.Target;
        }

        try
        {
            context.World.Configuration.WorldName.Value = worldName;
            return onSuccess.Target;
        }
        catch (Exception ex)
        {
            UniLogPlugin.Error($"error in setting world thing: {ex}");
            return onFailure.Target;
        }
    }
}