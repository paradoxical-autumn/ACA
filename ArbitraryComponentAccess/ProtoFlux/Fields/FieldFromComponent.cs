using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldFromComponent : ObjectFunctionNode<FrooxEngineContext, IField>
{
    public ObjectInput<Component> Component;
    public ObjectInput<string> Field;

    protected override IField Compute(FrooxEngineContext context)
    {
        Component my_com = Component.Evaluate(context);
        string my_field = Field.Evaluate(context);

        if (my_com == null || my_field == null) return null;

        IField field = my_com.TryGetField(my_field);

        return field;

        //if (pi is T)
        //{
        //    return (T)pi;
        //}
        //return null;
    }
}