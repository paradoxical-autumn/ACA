using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.EngineShennanigans;

[NodeCategory("EngineShennanigans")]
public class GetFromSyncElementList<T> : VoidNode<FrooxEngineContext> where T : class, ISyncMember, new()
{
    public ObjectInput<SyncElementList<T>> syncElementList;
    public ValueInput<int> index;
    public ObjectOutput<ISyncMember> isyncMember;
    public ValueOutput<int> length;

    protected override void ComputeOutputs( FrooxEngineContext ctx )
    {
        SyncElementList<T>? syncElementList = this.syncElementList!.Evaluate( ctx );
        int index = this.index.Evaluate( ctx, 0 );

        if ( syncElementList == null )
        {
            length.Write( 0, ctx );
            isyncMember.Write( null!, ctx );
            return;
        }

        length.Write( syncElementList.Count, ctx );

        if ( syncElementList.Count > index && index >= 0)
        {
            isyncMember.Write( syncElementList.GetElement( index ), ctx );
        }
        else
        {
            isyncMember.Write( null!, ctx );
        }
    }

    public GetFromSyncElementList()
    {
        length = new( this );
        isyncMember = new( this );
    }
}