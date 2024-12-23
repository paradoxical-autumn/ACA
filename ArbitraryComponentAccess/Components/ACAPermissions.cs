using FrooxEngine;
using Elements.Core;
using System.Security.Policy;
using FrooxEngine.CommonAvatar;

namespace ArbitraryComponentAccess.Components;

[Category("ACA")]
public class ACAPermissions : PermissionsComponent
{
    public readonly Sync<bool> is_ACA_Allowed = new();
    public readonly SyncTypeList BlockedTypes = new();
    public override int Version => 2;

    // ACA is feckin dangerous! so we're giving rule breakers dangerous punishments :3c ~ autumn
    public override PermissionViolationAction DefaultViolationAction => PermissionViolationAction.TempBanUser;

    public override void Cleanup(User user, Worker worker)
    {
        
    }

    public override bool Validate(User user, Worker worker)
    {
        return true;
    }

    public override bool ValidatesType(Type type)
    {
        return false;
    }

    public void SetupPermissions()
    {
        // default aca allowed to true
        is_ACA_Allowed.Value = true;

        // prevent ACAing into this component
        BlockedTypes.AddUnique(typeof(ACAPermissions));

        // all other permissions
        BlockedTypes.AddUnique(typeof(AvatarObjectPermissions));
        BlockedTypes.AddUnique(typeof(CameraPermissions));
        BlockedTypes.AddUnique(typeof(GrabbablePermissions));
        BlockedTypes.AddUnique(typeof(InteractionHandlerPermissions));
        BlockedTypes.AddUnique(typeof(LocomotionPermissions));
        BlockedTypes.AddUnique(typeof(ScreenViewPermissions));
        BlockedTypes.AddUnique(typeof(TouchablePermissions));
        BlockedTypes.AddUnique(typeof(UserPermissions));
        BlockedTypes.AddUnique(typeof(VoicePermission));
        BlockedTypes.AddUnique(typeof(WorldPermissions));

        // other types that should be locked
        BlockedTypes.AddUnique(typeof(SimpleAvatarProtection));
    }

    protected override void OnAttach()
    {
        base.OnAttach();
        SetupPermissions();
    }

    public bool IsTypeAllowedForExecution(Type type)
    {
        foreach (Type btype in BlockedTypes)
        {
            if (btype == null)
                continue;

            if (btype == type)
            {
                return false;
            }
        }

        return true;
    }
}