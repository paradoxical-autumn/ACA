// no auto
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldToValueLogix<T> : ValueFunctionNode<FrooxEngineContext, T> where T : unmanaged
{
    public ObjectInput<IField> field;

    protected override T Compute(FrooxEngineContext context)
    {
        IField? field = this.field!.Evaluate(context);

        if (field == null) 
            return default;

        if (field.BoxedValue is T t)
        {
            return t;
        }

        return default;
    }
}