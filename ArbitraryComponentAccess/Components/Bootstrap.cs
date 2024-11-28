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

    private static void HandleThreadFinished(object sender, EventArgs e)
    {
        UniLogPlugin.Log("welcome back to main!!");
        UserspaceRadiantDash URD = Userspace.UserspaceWorld?.GetGloballyRegisteredComponent<UserspaceRadiantDash>()!;
        ModalOverlayManager mgr = URD.Slot.GetComponent<ModalOverlayManager>();

        RectTransform rect = mgr.OpenModalOverlay(new float2(0.6f, 0.6f), "ACA Bootstrapper", false, true)!;
        if (rect != null)
        {
            ACAWarningDialog obj = rect.Slot.AttachComponent<ACAWarningDialog>();
        }
        else
        {
            UniLogPlugin.Error("erm the rect was null??");
        }
    }

    static ExecutionHook()
    {
        UniLogPlugin.Log("Start of execution hook!!");

        UserspaceCheckWorker worker = new UserspaceCheckWorker();
        worker.JobFinished += HandleThreadFinished;

        Thread thread1 = new Thread(worker.Run);
        thread1.Start();
}

    class UserspaceCheckWorker
    {
        public event EventHandler JobFinished;

        public void Run()
        {
            Thread.CurrentThread.IsBackground = true;
            while (Userspace.UserspaceWorld == null)
            {
                UniLogPlugin.Log("userspace world not there!!");
                Thread.Sleep(1000);
            }

            UniLogPlugin.Log("USERSPACE WORLD THERE!!");

            UniLogPlugin.Log("checking for URD");
            UserspaceRadiantDash URD = Userspace.UserspaceWorld?.GetGloballyRegisteredComponent<UserspaceRadiantDash>()!;

            while (URD == null)
            {
                UniLogPlugin.Log("urd null, checking in 1sec");
                Thread.Sleep(1000);
                URD = Userspace.UserspaceWorld?.GetGloballyRegisteredComponent<UserspaceRadiantDash>()!;
            }

            UniLogPlugin.Log("urd isnt null anymore :D -- checking MOM (modal overlay mgr, not mommy)");

            while (URD.Slot.GetComponent<ModalOverlayManager>() == null)
            {
                UniLogPlugin.Log("erm overlay returned null so it no worky :P -- trying again in 1 sec");
                Thread.Sleep(1000);
            }

            JobFinished(this, EventArgs.Empty);
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