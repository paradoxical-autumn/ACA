using ArbitraryComponentAccess.Components;
using Elements.Core;
using FrooxEngine;

namespace ArbitraryComponentAccess;

// This is such a fucking hack but since Engine.Current.OnReady now runs BEFORE userspace initialization
// I have to do this bullshit where we wait for userspace to start up.
// I wish we had an Engine.Current.OnUserspaceInitialised or whatever
// or, ever better, Userspace.Initialised()
// ~ prdx.

// upvote github issue https://github.com/Yellow-Dog-Man/Resonite-Issues/issues/3457
// please.

internal class ACAInitialiser
{
    internal static void Initialise()
    {
        UniLogPlugin.Log("Start of ACAInitialiser");

        try
        {
            Engine.Current.OnReady += async () =>
            {
                UniLogPlugin.Log("Hooked into OnReady successfully! Attempting userspace editing.");
                World userspaceWorld = Userspace.UserspaceWorld;
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                // as with the splittening, this shit is no longer guaranteed false.
                // fuckkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk ~ prdx
                while (userspaceWorld == null)
                {
                    UniLogPlugin.Warning("Userspace world is null. Waiting a bit.", false);
                    await Task.Delay(100);
                    userspaceWorld = Userspace.UserspaceWorld;
                }
                
                // UniLogPlugin.Log("Userspace world loaded?????? Maybe? I'm praying to god here. ~ prdx");

                userspaceWorld.RunSynchronously(() =>
                {
                    // UniLogPlugin.Log("calling from inside userspaceWorld.RunSync() hehe ~ prdx");
                    Userspace.UserspaceWorld.RunSynchronously(delegate
                    {
                        // UniLogPlugin.Log("calling from inside Userspace.UserspaceWorld.RunSync() who the fuck wrote this code? ~ prdx");
                        Slot slot = Userspace.UserspaceWorld.AddSlot("ACA Warning", false);
                        slot.PositionInFrontOfUser(float3.Backward);
                        slot.AttachComponent<ACAWarningPopup>();
                    });
                });

            };
        }
        catch (Exception ex)
        {
            UniLogPlugin.Warning($"Exception thrown during init: {ex}", true);
        }
    }
}