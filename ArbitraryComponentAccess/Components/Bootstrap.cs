using FrooxEngine;
using System.Reflection;

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
        UniLogPlugin.Log("Start of execution hook!!");
        while (true)
        {
            if (Userspace.UserspaceWorld != null)
            {
                break;
            }
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