using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldToStringLogix : ObjectFunctionNode<FrooxEngineContext, string>
{
    public ObjectInput<IField<string>> field;

    protected override string Compute(FrooxEngineContext context)
    {
        IField? field = this.field!.Evaluate(context);

        if (field == null) 
            return default!;

        if (field.BoxedValue is string t)
        {
            return t;
        }

        return default!;
    }
}