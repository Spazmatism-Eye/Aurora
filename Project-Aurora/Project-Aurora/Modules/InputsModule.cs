using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Lombok.NET;
using Aurora.Modules.Inputs;
using Aurora.Settings;
using Common.Devices;
using Common.Utils;

namespace Aurora.Modules;

public sealed partial class InputsModule : AuroraModule
{
    private const bool VolumeToBrightnessReady = false;
    
    public override async Task InitializeAsync()
    {
        await Initialize();
    }

    protected override Task Initialize()
    {
        if (Global.Configuration.EnableInputCapture)
        {
            Global.logger.Information("Loading Input Hooking");
            Global.InputEvents = new InputEvents();
            if (VolumeToBrightnessReady)
            {
                Global.Configuration.PropertyChanged += SetupVolumeAsBrightness;
                SetupVolumeAsBrightness(Global.Configuration,
                    new PropertyChangedEventArgs(nameof(Global.Configuration.UseVolumeAsBrightness)));
            }
            Global.logger.Information("Loaded Input Hooking");
        }

        DesktopUtils.StartSessionWatch();

        Global.key_recorder = new KeyRecorder(Global.InputEvents);
        return Task.CompletedTask;
    }

    [Async]
    public override void Dispose()
    {
        Global.key_recorder?.Dispose();
        Global.InputEvents.Dispose();
    }

    private static void SetupVolumeAsBrightness(object? sender, PropertyChangedEventArgs eventArgs)
    {
        if (eventArgs.PropertyName != nameof(Global.Configuration.UseVolumeAsBrightness)) return;
        if (Global.Configuration.UseVolumeAsBrightness)
        {
            Global.InputEvents.KeyDown -= InterceptVolumeAsBrightness;
            Global.InputEvents.KeyDown += InterceptVolumeAsBrightness;
        }
        else
        {
            Global.InputEvents.KeyDown -= InterceptVolumeAsBrightness;
        }
    }
    
    private static void InterceptVolumeAsBrightness(object? sender, KeyboardKeyEvent e)
    {
        if (!Global.InputEvents.Alt) return;
        
        e.Intercepted = true;
        Task.Factory.StartNew(() =>
            {
                var brightness = Global.Configuration.GlobalBrightness;
                brightness += e.GetDeviceKey() == DeviceKeys.VOLUME_UP ? 0.05f : -0.05f;
                Global.Configuration.GlobalBrightness = Math.Max(0f, Math.Min(1f, brightness));

                ConfigManager.Save(Global.Configuration, Configuration.ConfigFile);
            }
        );
    }
}