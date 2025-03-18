// no auto
using ArbitraryComponentAccess.Components;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

[NodeCategory("ACA/Components")]
public class ForeachComponentLogix : ActionNode<FrooxEngineContext>
{
    public ObjectInput<Slot> slot;
    public readonly ObjectOutput<Component> component;
    public Call loopStart;
    public Call loopIteration;
    public Continuation loopEnd;
    public Continuation onBlocked;
    public override bool CanBeEvaluated => false;

    protected override IOperation Run(FrooxEngineContext context)
    {
        Slot? slot = this.slot!.Evaluate(context);

        if (!context.World.Permissions.CheckAll((ACAPermissions p) => p.is_ACA_Allowed))
            return onBlocked.Target;

        loopStart.Execute(context);

        if (slot != null)
        {
            slot.ForeachComponent<Component>
            ( (c) => 
                { 
                    if (context.World.Permissions.CheckAll((ACAPermissions p) => p.IsTypeAllowedForExecution(c.GetType())))
                    {
                        component.Write(c, context);
                    }
                    else
                    {
                        component.Write(null!, context);
                    }
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
