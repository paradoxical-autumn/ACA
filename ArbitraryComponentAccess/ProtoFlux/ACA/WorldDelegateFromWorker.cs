using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA")]
public class BoxedWorldDelegateFromIWorldElementLogix : ObjectFunctionNode<FrooxEngineContext, object>
{
    public ObjectInput<IWorldElement> iWorldElement;
    public ObjectInput<string> delegateName;

    protected override object Compute(FrooxEngineContext context)
    {
        IWorldElement? cObj = iWorldElement!.Evaluate(context);
        string? cName = delegateName!.Evaluate(context);
        return new WorldDelegate( cObj?.ReferenceID ?? 0, cName ?? "", null! );
    }
}