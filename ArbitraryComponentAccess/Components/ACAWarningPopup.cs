using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using System.Diagnostics;

namespace ArbitraryComponentAccess.Components;

[ExceptionHandling(ExceptionAction.DestroySlot)]
public class ACAWarningPopup : Component
{
    public override bool UserspaceOnly => true;

    protected override void OnAttach()
    {
        base.OnAttach();
        UIBuilder uiBuilder = RadiantUI_Panel.SetupPanel(base.Slot, "Arbitrary Component Access", new float2(1280f, 720f), pinButton: false);
        Slot slot = base.Slot;
        float3 v = slot.LocalScale;
        slot.LocalScale = v * 0.0005f;
        RadiantUI_Constants.SetupEditorStyle(uiBuilder);
        uiBuilder.VerticalLayout(2f);
        uiBuilder.Style.MinHeight = 550f;
        // "<b><color=red>Warning!!</closeall>\nYou are currently running ACA. It is <b>not</b> advised to join random sessions or play with people you do <b>not</b> trust. The authors of the plugin take no liability for any damages caused.\n\nPlease see: https://github.com/paradoxical-autumn/ArbitraryComponentAccess for more information."
        LocaleString text = "<b><color=red>Warning!!</closeall>\nYou are currently running ACA. It is <b>not</b> advised to join random sessions or play with people you do <b>not</b> trust. The authors of the plugin take no liability for any damages caused.";
        uiBuilder.Text(in text);
        uiBuilder.Style.MinHeight = 32f;
        uiBuilder.HorizontalLayout(4f);
        text = "Okay.";
        colorX? tint = RadiantUI_Constants.Sub.RED;
        Button target = uiBuilder.Button(in text, in tint, Dismiss);
        text = "Read more";
        tint = RadiantUI_Constants.Sub.CYAN;
        uiBuilder.Button(in text, in tint, OpenGithub);

        RunInUpdates(2, delegate
        {
            Slot temp = base.World.AddSlot("TEMP");
            temp.GlobalPosition = float3.Up;
            Slot prevParent = base.Slot.Parent;
            base.Slot.Parent = temp;
            RunInUpdates(2, delegate
            {
                base.Slot.Parent = prevParent;
                temp.Destroy();
            });
        });
    }

    private void Dismiss(IButton button, ButtonEventData eventData)
    {
        if (base.World == Userspace.UserspaceWorld)
        {
            base.Slot.Destroy();
        }
    }

    private void OpenGithub(IButton button, ButtonEventData eventData)
    {
        Uri URL = new("https://github.com/paradoxical-autumn/ArbitraryComponentAccess");

        if (base.World != Userspace.UserspaceWorld)
        {
            return;
        }

        if (URL != null && (URL.Scheme == "http" || URL.Scheme == "https"))
        {
            RunInBackground(delegate
            {
                Process.Start(URL.ToString());
            });
        }
    }
}