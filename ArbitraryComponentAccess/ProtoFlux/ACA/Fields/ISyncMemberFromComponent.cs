using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Fields;

[NodeCategory("ACA/Fields")]
public class ISyncMemberFromComponentLogix : ObjectFunctionNode<FrooxEngineContext, ISyncMember>
{
    public ObjectInput<Component> component;
    public ObjectInput<string> fieldName;

    protected override ISyncMember Compute(FrooxEngineContext context)
    {
        Component? my_com = component!.Evaluate(context);
        string? my_field = fieldName!.Evaluate(context);

        if (my_com == null || my_field == null) return null!;

        ISyncMember field = my_com.GetSyncMember( my_field );

        return field;
    }
}