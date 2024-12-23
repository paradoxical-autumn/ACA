using FrooxEngine;
using Elements.Core;
using System.Security.Policy;

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
        is_ACA_Allowed.Value = true;
        BlockedTypes.AddUnique(typeof(ACAPermissions));
    }

    protected override void OnAttach()
    {
        base.OnAttach();
        SetupPermissions();
    }

    public bool CanExecute(Component comp)
    {
        return CheckSlotForExecution(comp.Slot);
    }
    public bool CanExecute(Slot slot)
    {
        return CheckSlotForExecution(slot);
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

    private bool CheckSlotForExecution(Slot slot)
    {
        foreach (Type btype in BlockedTypes)
        {
            if (btype == null)
                continue;

            if (slot?.GetComponentInParents(btype) != null)
            {
                return false;
            }
        }

        return true;
    }
}