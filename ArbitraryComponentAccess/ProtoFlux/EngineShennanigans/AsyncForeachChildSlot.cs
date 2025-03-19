using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.EngineShennanigans;

// Revive of EngineShennanigans POG?
[NodeCategory("EngineShennanigans")]
public class AsyncForeachChildSlotLogix : AsyncActionNode<FrooxEngineContext>
{
    public ObjectInput<Slot> slot;
    public ValueInput<bool> reverse;
    public readonly ObjectOutput<Slot> childSlot;
    public AsyncCall loopStart;
    public AsyncCall loopIteration;
    public Continuation loopEnd;
    public override bool CanBeEvaluated => false;

    protected override async Task<IOperation> RunAsync(FrooxEngineContext context)
    {
        Slot? slot = this.slot!.Evaluate(context);
        bool reverse = this.reverse.Evaluate(context);

        await loopStart.ExecuteAsync(context);

        if (slot != null)
        {
            if ( !reverse )
            {
                for ( int i = 0; i < slot.ChildrenCount; i++ )
                {
                    childSlot.Write( slot[i], context ); 
                    await loopIteration.ExecuteAsync( context ); 
                    if ( context.AbortExecution )
                        throw new ExecutionAbortedException( Runtime as IExecutionRuntime, this, loopIteration.Target, isAsync: false );

                }
            }
            else
            {
                for ( int i = slot.ChildrenCount-1; i >= 0; i-- )
                {
                    childSlot.Write( slot[i], context ); 
                    await loopIteration.ExecuteAsync( context ); 
                    if ( context.AbortExecution )
                        throw new ExecutionAbortedException( Runtime as IExecutionRuntime, this, loopIteration.Target, isAsync: false );
                }
            }
        }

        return loopEnd.Target;
    }

    public AsyncForeachChildSlotLogix()
    {
        childSlot = new(this);
    }
}
