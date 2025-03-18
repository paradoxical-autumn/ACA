using ArbitraryComponentAccess.Components;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class FindComponentInChildrenLogix : ObjectFunctionNode<FrooxEngineContext, Component>
{
    public ObjectInput<Slot> slot;
    public ObjectInput<Type> componentType;
    public ValueInput<int> componentIndex;
    public ValueInput<bool> self;

    protected override Component Compute(FrooxEngineContext context)
    {
        Type? ctxType = componentType!.Evaluate(context);

        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.is_ACA_Allowed))
        {
            return null!;
        }
        if (ctxType == null || !context.World.Permissions.CheckAll((ACAPermissions p) => p.IsTypeAllowedForExecution( ctxType )))
        {
            return null!;
        }

        Slot? ctxslot = slot!.Evaluate(context);
        int indx = componentIndex.Evaluate(context);
        bool ctxself = self.Evaluate(context);

        // handle nulls
        if (ctxslot == null || ctxslot.IsRemoved ) return null!;

        return ctxslot.GetComponentInChildren<Component>( (c) => c.GetType() == ctxType && indx-- <= 0, ctxself );
    }
}