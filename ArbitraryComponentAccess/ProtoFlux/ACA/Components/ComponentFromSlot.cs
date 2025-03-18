// no auto
using ArbitraryComponentAccess.Components;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class ComponentFromSlotLogix : ObjectFunctionNode<FrooxEngineContext, Component>
{
    public ObjectInput<Slot> slot;
    public ObjectInput<Type> componentType;
    public ValueInput<int> componentIndex;

    protected override Component Compute(FrooxEngineContext context)
    {
        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.is_ACA_Allowed))
        {
            return null!;
        }

        Slot? my_slot = slot!.Evaluate(context);
        Type? my_type = componentType!.Evaluate(context);
        int my_index = componentIndex!.Evaluate(context, 0);

        // handle nulls
        if (my_slot == null) return null!;
        if (my_type == null) return null!;

        if (!my_type.IsSubclassOf(typeof(Component)) || my_type.GetConstructor(Type.EmptyTypes) == null || my_type.ContainsGenericParameters)
            return null!;

        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.IsTypeAllowedForExecution(my_type)))
        {
            return null!;
        }

        List<Component> my_comps = my_slot.GetComponents(my_type.ToString()).ToList();

        if (my_index < 0 || my_index >= my_comps.Count) return null!;

        return my_comps.GetOrNull(my_index);
    }
}