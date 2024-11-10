// no auto
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class ComponentFromSlotLogix<T> : ObjectFunctionNode<FrooxEngineContext, T> where T : Component
{
    public ObjectInput<Slot> slot;
    public ValueInput<int> componentIndex;

    protected override T Compute(FrooxEngineContext context)
    {
        Slot? my_slot = slot!.Evaluate(context);
        int my_index = componentIndex.Evaluate(context);

        // handle nulls
        if (my_slot == null) return null!;

        List<T> my_components = my_slot.GetComponents<T>();

        return my_components.GetOrNull( my_index );
    }
}