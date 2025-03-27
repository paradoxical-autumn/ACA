using FrooxEngine;
using FrooxEngine.CommonAvatar;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ArbitraryComponentAccess.ProtoFlux.EngineShennanigans;

[NodeCategory("EngineShennanigans")]
public class GetAvatarRoot : ObjectFunctionNode<FrooxEngineContext, AvatarRoot>
{
    public ObjectInput<User> user;

    protected override AvatarRoot Compute( FrooxEngineContext ctx )
    {
        Slot? slot = user!.Evaluate( ctx )?.Root.Slot;

        if (slot == null)
            return null!;

        var avi = slot.GetComponentInChildren<AvatarObjectSlot>();
        return avi?.Equipped.Target as AvatarRoot ?? null!;
    }
}