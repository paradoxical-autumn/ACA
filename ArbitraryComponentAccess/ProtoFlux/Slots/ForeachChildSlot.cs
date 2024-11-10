// no auto
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

public class ForeachChildSlotLogix : ActionNode<FrooxEngineContext>
{
    public ObjectInput<Slot> slot;
    public ValueInput<bool> reverse;
    public readonly ObjectOutput<Slot> childSlot;
    public Call loopStart;
    public Call loopIteration;
    public Continuation loopEnd;
    public override bool CanBeEvaluated => false;

    protected override IOperation Run(FrooxEngineContext context)
    {
        Slot? slot = this.slot!.Evaluate(context);
        bool reverse = this.reverse.Evaluate(context);

        loopStart.Execute(context);

        if (slot != null)
        {
            if ( !reverse )
            {
                for ( int i = 0; i < slot.ChildrenCount; i++ )
                {
                    childSlot.Write( slot[i], context ); 
                    loopIteration.Execute( context ); 
                    if ( context.AbortExecution )
                        throw new ExecutionAbortedException( Runtime as IExecutionRuntime, this, loopIteration.Target, isAsync: false );

                }
            }
            else
            {
                for ( int i = slot.ChildrenCount-1; i >= 0; i-- )
                {
                    childSlot.Write( slot[i], context ); 
                    loopIteration.Execute( context ); 
                    if ( context.AbortExecution )
                        throw new ExecutionAbortedException( Runtime as IExecutionRuntime, this, loopIteration.Target, isAsync: false );
                }
            }
        }

        return loopEnd.Target;
    }

    public ForeachChildSlotLogix()
    {
        childSlot = new(this);
    }
}
