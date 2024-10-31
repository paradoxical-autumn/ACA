using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldFromComponent<T> : ObjectFunctionNode<FrooxEngineContext, T> where T : class, IWorldElement
{
    public ObjectInput<Component> Component;
    public ObjectInput<string> Field;

    protected override T Compute(FrooxEngineContext context)
    {
        Component my_com = Component.Evaluate(context);
        string my_field = Field.Evaluate(context);

        if (my_com == null || my_field == null) return null;

        object pi = ACAReflections.GetProp(my_com, my_field);

        if (pi is T)
        {
            return (T)pi;
        }
        return null;
    }
}