using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldToElement : ObjectFunctionNode<FrooxEngineContext, IWorldElement>
{
    public ObjectInput<IField> Field;

    protected override IWorldElement Compute(FrooxEngineContext context)
    {
        IField my_field = Field.Evaluate(context);
        if (my_field == null) return default;

        return my_field as IWorldElement;
    }
}