using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Components;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Components;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/Slots" )]
public class ForeachChildSlot : ActionNode<FrooxEngineContext>
{
    public readonly SyncRef<INodeObjectOutput<Slot>> slot = new();
    public readonly SyncRef<INodeValueOutput<bool>> reverse = new();
    public readonly NodeObjectOutput<Slot> childSlot = new();
    public readonly SyncRef<ISyncNodeOperation> loopStart = new();
    public readonly SyncRef<ISyncNodeOperation> loopIteration = new();
    public readonly SyncRef<INodeOperation> loopEnd = new();
    
    public ForeachChildSlotLogix TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( ForeachChildSlotLogix );
    public override INode NodeInstance => TypedNodeInstance!;
    public override int NodeInputCount => base.NodeInputCount + 2;
    public override int NodeOutputCount => base.NodeOutputCount + 1;
    public override int NodeImpulseCount => base.NodeImpulseCount + 3;

    public override void ClearInstance()
    {
        TypedNodeInstance = null!;
    }

    public override N Instantiate<N>()
    {
        if ( TypedNodeInstance != null )
        {
            throw new InvalidOperationException( "Node has already been instantiated" );
        }

        TypedNodeInstance = new ForeachChildSlotLogix();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is ForeachChildSlotLogix typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( ForeachChildSlotLogix ) );
    }

    protected override ISyncRef GetInputInternal( ref int index )
    {
        ISyncRef inputInternal = base.GetInputInternal( ref index );
        if ( inputInternal != null )
        {
            return inputInternal;
        }

        switch ( index )
        {
            case 0:
                return slot;
            case 1:
                return reverse;
            default:
                index -= 2;
                return null!;
        }
    }

    protected override INodeOutput GetOutputInternal( ref int index )
    {
        INodeOutput outputInternal = base.GetOutputInternal( ref index );
        if ( outputInternal != null )
        {
            return outputInternal;
        }

        switch ( index )
        {
            case 0:
                return childSlot;
            default:
                index -= 1;
                return null!;
        }
    }

    protected override ISyncRef GetImpulseInternal( ref int index )
    {
        ISyncRef impulseInternal = base.GetImpulseInternal( ref index );
        if ( impulseInternal != null )
        {
            return impulseInternal;
        }
        switch ( index )
        {
        case 0:
            return loopStart;
        case 1:
            return loopIteration;
        case 2:
            return loopEnd;
        default:
            index -= 3;
            return null!;
        }
    }

    // FrooxEngine Weaver Stuff:
    protected override void InitializeSyncMembers()
    {
        base.InitializeSyncMembers();
    }

    public override ISyncMember GetSyncMember(int index)
    {
        return index switch
        {
            0 => persistent, 
            1 => updateOrder, 
            2 => EnabledField,
            3 => slot,
            4 => reverse,
            5 => childSlot,
            6 => loopStart,
            7 => loopIteration,
            8 => loopEnd,
            _ => throw new ArgumentOutOfRangeException(), 
        };
    }

    public static ForeachChildSlot __New()
    {
        return new ForeachChildSlot();
    }
}
