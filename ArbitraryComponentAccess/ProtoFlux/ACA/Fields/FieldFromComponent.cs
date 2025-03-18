// no auto
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class FieldFromComponentLogix : ObjectFunctionNode<FrooxEngineContext, IField>
{
    public ObjectInput<Component> component;
    public ObjectInput<string> fieldName;

    protected override IField Compute(FrooxEngineContext context)
    {
        Component? my_com = component!.Evaluate(context);
        string? my_field = fieldName!.Evaluate(context);

        if (my_com == null || my_field == null) return null!;

        IField field = my_com.TryGetField( my_field );

        return field;
    }
}