using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldToValue<T> : ValueFunctionNode<FrooxEngineContext, T> where T : unmanaged
{
    public ObjectInput<IField> Field;

    protected override T Compute(FrooxEngineContext context)
    {
        if (Field.Evaluate(context) == null) return default;

        if (Field.Evaluate(context).BoxedValue is T t)
        {
            return (T) t;
        }
        return default;
    }
}