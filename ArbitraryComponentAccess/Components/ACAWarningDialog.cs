using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;

namespace ArbitraryComponentAccess.Components;

[ExceptionHandling(ExceptionAction.DestroySlot)]
[GloballyRegistered]
public class ACAWarningDialog : Component
{
    public readonly SyncDelegate<Action<ACAWarningDialog>> CustomStart = new();

    public readonly Sync<bool> _uiBuilt = new();

    protected readonly Sync<bool> _initialized = new();

    public static ACAWarningDialog OpenDialogWindow(Slot root, LocaleString title = default(LocaleString))
    {
        title = "ArbitraryComponentAccess";

        UIBuilder uiBuilder = RadiantUI_Panel.SetupPanel(root, title, new float2(800f, 640f));
        float3 v = root.LocalScale;
        root.LocalScale = v * 0.0005f;
        RadiantUI_Constants.SetupEditorStyle(uiBuilder);
        return uiBuilder.Root.AttachComponent<ACAWarningDialog>();
    }

    public void BuildUI(Action<ACAWarningDialog> customStart = null!, Action<UIBuilder> customPresetUI = null!)
    {
        Action<ACAWarningDialog> customStart2 = customStart;
        Action<UIBuilder> customPresetUI2 = customPresetUI;

        if ((bool)_uiBuilt)
        {
            throw new InvalidOperationException("UI is already built.");
        }
        _uiBuilt.Value = true;

        StartTask(async delegate
        {
            await BuildUI_Async(customStart2, customPresetUI2);
        });
        base.Slot.SetContainerTitle("ArbitraryComponentAccess");
    }

    private async Task BuildUI_Async(Action<ACAWarningDialog> customStart = null!, Action<UIBuilder> customPresetUI = null!)
    {
        CustomStart.Target = customStart;
        if (base.IsDestroyed) return;

        UIBuilder ui = new UIBuilder(base.Slot);
        RadiantUI_Constants.SetupDefaultStyle(ui);
        RectTransform rect = ui.CurrentRect;
        //RectTransform rect = list[0];
        //RectTransform rect2 = list[2];

        ui = new UIBuilder(rect);
        //RadiantUI_Constants.SetupDefaultStyle(ui);
        //ui.HorizontalHeader(48f, out var header, out var content);

        //ui = new UIBuilder(header);
        RadiantUI_Constants.SetupDefaultStyle(ui);

        UIBuilder uIBuilder = ui;

        LocaleString text = "<b><color=red>Warning!!</closeall>\nYou are currently running ACA. It is <b>not</b> advised to join random sessions or play with people you do <b>not</b> trust. The authors of the plugin take no liability for any damages caused.\n\nPlease see: https://github.com/paradoxical-autumn/ArbitraryComponentAccess for more information.";
        uIBuilder.Text(in text);

        //ui = new UIBuilder(content);
        //RadiantUI_Constants
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (!_uiBuilt)
        {
            BuildUI();
        }
    }

    private void Close()
    {
        base.Slot.GetComponentInParents<IUIContainer>()?.CloseContainer();
    }
}
