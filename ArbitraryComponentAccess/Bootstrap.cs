using System.Runtime.CompilerServices;
using ArbitraryComponentAccess.Components;
using Elements.Core;
using FrooxEngine;

namespace ArbitraryComponentAccess;

public class ExecutionHook : IPlatformConnector {
#pragma warning disable CS1591
    
    public PlatformInterface Platform { get; private set; } = null!;
    public int Priority => -10;
    public string PlatformName => "ArbitrarilyContainedAnarchy";
    public string Username => null!;
    public string PlatformUserId => null!;
    public bool IsPlatformNameUnique => false;

    public void SetCurrentStatus(World world, bool isPrivate, int totalWorldCount) {}
    public void ClearCurrentStatus() {}
    public void Update() {}
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    public void NotifyOfLocalUser(User user) {}
    public void NotifyOfFile(string file, string name) {}
    public void NotifyOfScreenshot(World world, string file, ScreenshotType type, DateTime time) {}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<bool> Initialize(PlatformInterface platformInterface)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        UniLogPlugin.Log("Initialize() from platformInterface");
        Platform = platformInterface;
        return true;
    }
    
#pragma warning restore CS1591
#pragma warning disable CA2255

    [ModuleInitializer]
    public static void Init()
    {
        UniLogPlugin.Log("Init() from ModuleInitializer");
    }
    
#pragma warning restore CA2255
}