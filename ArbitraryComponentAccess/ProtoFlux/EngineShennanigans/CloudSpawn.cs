using FrooxEngine;
using FrooxEngine.ProtoFlux;

using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

using Elements.Core;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

// Revive of EngineShennanigans POG?
[NodeCategory("EngineShennanigans")]
public class CloudSpawnLogix : AsyncActionNode<FrooxEngineContext>
{
    public ObjectInput<Uri> recordURL;
    public ValueInput<float3> spawnPos;
    public ValueInput<floatQ> spawnRot;
    public ValueInput<float3> spawnScl;
    public ValueInput<bool> spawnPersistant;
    public ValueInput<bool> multiplyWithSavedScale;
    public ObjectInput<Slot> spawnParent;
    public readonly ObjectOutput<Slot> spawnedSlot;
    public AsyncCall onStartedSpawn;
    public Continuation onSpawned;
    public Continuation onFailed;

    public CloudSpawnLogix()
    {
        spawnedSlot = new(this);
    }

    protected override async Task<IOperation> RunAsync( FrooxEngineContext ctx )
    {
        await onStartedSpawn.ExecuteAsync( ctx );

        Uri? recordURL = this.recordURL!.Evaluate( ctx );
        Slot spawnParent = this.spawnParent!.Evaluate( ctx ) ?? ctx.World.RootSlot;
        float3 spawnPos = this.spawnPos.Evaluate( ctx );
        floatQ spawnRot = this.spawnRot.Evaluate( ctx );
        float3 spawnScl = this.spawnScl.Evaluate( ctx );
        bool spawnPersistant = this.spawnPersistant.Evaluate( ctx, true );
        bool multiplyWithSavedScale = this.multiplyWithSavedScale.Evaluate( ctx, true );
        
        if ( recordURL == null )
            return onFailed.Target;

        Slot slot = spawnParent.AddSlot( "Cloud Spawned", spawnPersistant );
        bool ok = await slot.LoadObjectAsync( recordURL, skipHolder: true );

        if ( multiplyWithSavedScale )
            slot.LocalScale *= spawnScl;
        else
            slot.LocalScale = spawnScl;

        slot.LocalPosition = spawnPos;
        slot.LocalRotation = spawnRot;

        if ( !ok )
        {
            slot.Destroy();
            spawnedSlot.Write( null!, ctx );
            return onFailed.Target;
        }

        spawnedSlot.Write( slot, ctx );

        return onSpawned.Target;
    }
}