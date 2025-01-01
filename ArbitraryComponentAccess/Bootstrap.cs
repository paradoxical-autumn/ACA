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
        UniLogPlugin.Log("Hooking into Engine.Current.OnReady.");
        try
        {
            Engine.Current.OnReady += () =>
                {
                    UniLogPlugin.Log("Hooked into OnReady successfully! Attempting userspace editing. This often leads to lock errors since we don't lock the datamodel. How do you even do that?");
                    World userspace_world = Userspace.UserspaceWorld;
                    if (userspace_world == null)
                    {
                        UniLogPlugin.Warning("Userspace world is null. Exiting...", true);
                        return;
                    }

                    Slot aca_slot = userspace_world.AddSlot("evidence ACA is working", false);
                };
        }
        catch (Exception ex)
        {
            UniLogPlugin.Warning($"Exception thrown during init: {ex}", true);
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