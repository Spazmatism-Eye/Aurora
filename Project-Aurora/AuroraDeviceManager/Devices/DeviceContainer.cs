using System.ComponentModel;
using System.Drawing;
using AuroraDeviceManager.Devices.RGBNet;
using Common;
using Common.Data;
using Common.Devices;
using Common.Utils;

namespace AuroraDeviceManager.Devices;

public sealed class DeviceContainer : IDisposable
{
    public IDevice Device { get; }

    private readonly SingleConcurrentThread _worker;

    private DeviceColorComposition _currentComp = new(new Dictionary<DeviceKeys, Color>());

    private readonly SemaphoreSlim _actionLock = new(1);

    private readonly MemorySharedStruct<DeviceInformation> _deviceInformation;
    private readonly MemorySharedArray<DeviceVariable> _deviceVariables;

    private string SharedObjectName => Device.DeviceName;

    public DeviceContainer(IDevice device)
    {
        Device = device;
        
        var args = new DoWorkEventArgs(null);
        _worker = new SingleConcurrentThread(device.DeviceName + " Thread", () => { WorkerOnDoWork(args).Wait(); });

        _deviceInformation = new MemorySharedStruct<DeviceInformation>(SharedObjectName, GetSharedDeviceInformation());
        _deviceInformation.UpdateRequested += (_, _) => { UpdateSharedMemory(); };

        var deviceVariables = CreateDeviceVariables();
        _deviceVariables = new MemorySharedArray<DeviceVariable>(SharedObjectName + "-vars", deviceVariables.Count);
        _deviceVariables.WriteCollection(deviceVariables);
    }

    private List<DeviceVariable> CreateDeviceVariables()
    {
        var deviceVariables = new List<DeviceVariable>();
        foreach (var (varName, item) in Device.RegisteredVariables.Variables)
        {
            var vt = item.Default switch
            {
                bool => VariableType.Boolean,
                int => VariableType.Int,
                long => VariableType.Long,
                float => VariableType.Float,
                double => VariableType.Double,
                string => VariableType.String,
                Color => VariableType.Color,
                DeviceKeys => VariableType.DeviceKeys,
                _ => VariableType.None,
            };
            Func<object?, byte[]?> convert = vt switch
            {
                VariableType.None => _ => null,
                VariableType.Boolean => data => data == null ? null : BitConverter.GetBytes((long)((bool)data ? 1 : 0)),
                VariableType.Float => data => data == null ? null : BitConverter.GetBytes((double)(float)data),
                VariableType.Double => data => data == null ? null : BitConverter.GetBytes((double)data),
                VariableType.Int => data => data == null ? null : BitConverter.GetBytes((long)(int)data),
                VariableType.Long => data => data == null ? null : BitConverter.GetBytes((long)data),
                VariableType.DeviceKeys => data => data == null ? null : BitConverter.GetBytes((long)(int)data),
                VariableType.String => _ => null,
                VariableType.Color => data => data == null ? null : BitConverter.GetBytes((long)((Color)data).ToArgb()),
            };

            var variable = new DeviceVariable(
                Device.DeviceName, varName,
                convert(item.Value),
                convert(item.Default),
                convert(item.Min),
                convert(item.Max),
                item.Title, item.Remark, (int)item.Flags, vt,
                vt == VariableType.String ? item.Value as string ?? "" : ""
            );
            deviceVariables.Add(variable);
        }

        return deviceVariables;
    }

    private Task WorkerOnDoWork(DoWorkEventArgs doWorkEventArgs)
    {
        using (_actionLock)
        {
            if (Device is { IsInitialized: false, IsDoingWork: false })
            {
                Device.Initialize();
                UpdateSharedMemory();
            }

            try
            {
                Device.UpdateDevice(_currentComp, doWorkEventArgs).Wait();
            }
            catch (Exception e)
            {
                Global.Logger.Error(e, "Device update thread error");
                UpdateSharedMemory();
            }
        }

        return Task.CompletedTask;
    }

    public void UpdateDevice(Dictionary<DeviceKeys, Color> keyColors)
    {
        _currentComp.KeyColors = keyColors;
        if (!Device.IsDoingWork)
        {
            _worker.Trigger();
        }
    }

    public async Task EnableDevice()
    {
        using (_actionLock)
        {
            var initTask = Device.Initialize();
            UpdateSharedMemory();
            if (await initTask)
            {
                Global.Logger.Information("[Device][{DeviceName}] Initialized Successfully", Device.DeviceName);
            }
            else
            {
                Global.Logger.Information("[Device][{DeviceName}] Failed to initialize", Device.DeviceName);
            } 
            UpdateSharedMemory();
        }
    }

    public async Task DisableDevice()
    {
        using (_actionLock)
        {
            var shutdownTask = Device.ShutdownDevice();
            UpdateSharedMemory();
            await shutdownTask;
            Global.Logger.Information("[Device][{DeviceName}] Shutdown", Device.DeviceName);
            UpdateSharedMemory();
        }
    }

    public void UpdateVariables()
    {
        Global.DeviceConfig.VarRegistry.Combine(Device.RegisteredVariables);

        var deviceVariables = CreateDeviceVariables();
        _deviceVariables.WriteCollection(deviceVariables);
    }

    public void Dispose()
    {
        _worker.Dispose(250);
    }

    private void UpdateSharedMemory()
    {
        _deviceInformation.WriteObject(GetSharedDeviceInformation());
    }

    private DeviceInformation GetSharedDeviceInformation()
    {
        return new DeviceInformation(Device.DeviceName,
            Device.DeviceDetails,
            Device.DeviceUpdatePerformance,
            Device.IsDoingWork,
            Device.IsInitialized,
            Device.GetDevices(),
            Device is RgbNetDevice
        );
    }
}