﻿using System.ComponentModel;
using System.Threading.Tasks;
using AuroraRgb.Modules.HardwareMonitor;
using AuroraRgb.Nodes;
using Lombok.NET;

namespace AuroraRgb.Modules;

public sealed partial class HardwareMonitorModule : AuroraModule
{
    protected override Task Initialize()
    {
        Global.Configuration.PropertyChanged += ConfigurationOnPropertyChanged;
        if (Global.Configuration.EnableHardwareInfo)
        {
            LocalPcInformation.HardwareMonitor = new HardwareMonitor.HardwareMonitor();
        }

        return Task.CompletedTask;
    }

    private void ConfigurationOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Global.Configuration.EnableHardwareInfo))
        {
            return;
        }

        LocalPcInformation.HardwareMonitor.Dispose();
        if (Global.Configuration.EnableHardwareInfo)
        {
            LocalPcInformation.HardwareMonitor = new HardwareMonitor.HardwareMonitor();
        }
        else
        {
            LocalPcInformation.HardwareMonitor = new NoopHardwareMonitor();
        }
    }


    [Async]
    public override void Dispose()
    {
        LocalPcInformation.HardwareMonitor.Dispose();
        LocalPcInformation.HardwareMonitor = new NoopHardwareMonitor();
    }
}