// no auto
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

public class ForeachComponentLogix : ActionNode<FrooxEngineContext>
{
    public ObjectInput<Slot> slot;
    public readonly ObjectOutput<Component> component;
    public Call loopStart;
    public Call loopIteration;
    public Continuation loopEnd;
    public override bool CanBeEvaluated => false;

    protected override IOperation Run(FrooxEngineContext context)
    {
        Slot? slot = this.slot!.Evaluate(context);

        loopStart.Execute(context);

        if (slot != null)
        {
            slot.ForeachComponent<Component>
            ( (c) => 
                { 
                    component.Write( c, context ); 
                    loopIteration.Execute( context ); 
                    return !context.AbortExecution;
                }, false, false );

            if ( context.AbortExecution )
                throw new ExecutionAbortedException( Runtime as IExecutionRuntime, this, loopIteration.Target, isAsync: false );
        }

        return loopEnd.Target;
    }

    public ForeachComponentLogix()
    {
        component = new(this);
    }
}
