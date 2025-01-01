using FrooxEngine;
using FrooxEngine.UIX;
using System.Threading;
using Elements.Core;
using ArbitraryComponentAccess.Components;

namespace ArbitraryComponentAccess;

[ImplementableClass(true)]
internal static class ExecutionHook
{
#pragma warning disable CS0169, IDE0051, CA1823, IDE0044
    // fields must exist due to reflective access
    private static Type? __connectorType;
    private static Type? __connectorTypes;

    private static DummyConnector InstantiateConnector()
    {
        return new DummyConnector();
    }
#pragma warning restore CS0169, IDE0051, CA1823, IDE0044

    static ExecutionHook()
    {
        UniLogPlugin.Log("Hooking into Engine.Current.OnReady. This may take a while -- PARADOX! BE PATIENT.");
        try
        {
            Engine.Current.OnReady += () =>
                {
                    UniLogPlugin.Log("Hooked into OnReady successfully! Trying funny userspace shid!");
                    UserspaceRadiantDash userspaceRadiantDash = Userspace.UserspaceWorld.GetRadiantDash();
                    if (userspaceRadiantDash != null )
                    {
                        // :3
                        UniLogPlugin.Log("we're not null!! yay!!");

                        // Grab the modal overlay and start using it! This does start a race condition, we need to be sure the dash is... well, loaded.
                        // We could spawn the dialogue in userspace using another system
                        // Wait that's a better idea to prevent race conditions.
                        // Why aren't we using that?
                        ModalOverlayManager mgr = userspaceRadiantDash.Slot.GetComponent<ModalOverlayManager>();

                        RectTransform rect = mgr.OpenModalOverlay(new float2(0.6f, 0.6f), "ACA Bootstrapper", false, false);
                        if (rect != null )
                        {
                            ACAWarningDialog obj = rect.Slot.AttachComponent<ACAWarningDialog>();
                        }
                        else
                        {
                            UniLogPlugin.Error("Rect was null!", true);
                        }
                    }
                    else
                    {
                        // all hell has broken lose.
                        UniLogPlugin.Warning("URD is null. Wtf?", true);
                    }
                };
        }
        catch (Exception ex)
        {
            UniLogPlugin.Warning($"Non-fatal thrown, then caught, during init: {ex}", true);
        }
    }

    private sealed class DummyConnector : IConnector
    {
#pragma warning disable CS8766
        public IImplementable? Owner { get; private set; }
#pragma warning restore CS8766
        public void ApplyChanges() { }
        public void AssignOwner(IImplementable owner) => Owner = owner;
        public void Destroy(bool destroyingWorld) { }
        public void Initialize() { }
        public void RemoveOwner() => Owner = null;
    }
}