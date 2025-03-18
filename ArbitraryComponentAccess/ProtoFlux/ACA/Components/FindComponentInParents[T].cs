using ArbitraryComponentAccess.Components;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class FindComponentInParentsLogix<T> : ObjectFunctionNode<FrooxEngineContext, T> where T : Component
{
    public ObjectInput<Slot> slot;
    public ValueInput<int> componentIndex;
    public ValueInput<bool> self;

    protected override T Compute(FrooxEngineContext context)
    {
        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.is_ACA_Allowed))
        {
            return null!;
        }
        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.IsTypeAllowedForExecution(typeof(T))))
        {
            return null!;
        }

        Slot? ctxslot = slot!.Evaluate(context);
        int indx = componentIndex.Evaluate(context);
        bool ctxself = self.Evaluate(context);

        // handle nulls
        if (ctxslot == null || ctxslot.IsRemoved ) return null!;

        return ctxslot.GetComponentInParents<T>( (c) => indx-- <= 0, ctxself );
    }
}