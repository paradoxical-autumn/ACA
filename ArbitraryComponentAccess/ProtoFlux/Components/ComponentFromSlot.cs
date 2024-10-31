using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ProtoFlux.Runtimes.Execution.Nodes.ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class ComponentFromSlot<T> : ObjectFunctionNode<FrooxEngineContext, T> where T : Component
{
    public ObjectInput<Slot> Slot;
    public ValueInput<int> ComponentIndex;

    protected override T Compute(FrooxEngineContext context)
    {
        Slot my_slot = Slot.Evaluate(context);
        int my_index = ComponentIndex.Evaluate(context);

        // handle nulls
        if (my_slot == null) return default(T);

        //List<T> comp_list = Pool.BorrowList<T>();
        List<T> my_components = my_slot.GetComponents<T>();

        //return my_components.FirstOrDefault(); // old: return default comp

        // handle index errors
        if (my_index >= my_components.Count) return default(T);

        return my_components[my_index];
    }
}