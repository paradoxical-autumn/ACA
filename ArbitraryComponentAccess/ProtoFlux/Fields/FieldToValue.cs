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
        IField? field = Field!.Evaluate(context);
        if (field == null) return default;

        if (field.BoxedValue is T t)
        {
            return (T) t;
        }
        return default;
    }
}