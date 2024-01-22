using System;
using System.Management;
using System.Security.Principal;
using System.Windows;
using Microsoft.Win32;

namespace Aurora.Utils;

public class RegistryChangedEventArgs(object data) : EventArgs
{
    public readonly object? Data = data;
}

public enum RegistryHiveOpt
{
    CurrentUser,
    LocalMachine,
}

public sealed class RegistryWatcher(RegistryHiveOpt registryHive, string key, string value) : IDisposable
{
    public event EventHandler<RegistryChangedEventArgs>? RegistryChanged;

    private ManagementEventWatcher? _eventWatcher;

    public void StartWatching()
    {
        if (_eventWatcher != null)
        {
            SendData();
            return;
        }

        var keyPath = key.Replace(@"\", @"\\");
        var queryString = registryHive switch
        {
            RegistryHiveOpt.LocalMachine =>
                $"SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_LOCAL_MACHINE' AND KeyPath='{keyPath}' AND ValueName='{value}'",
            RegistryHiveOpt.CurrentUser =>
                $@"SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{WindowsIdentity.GetCurrent().User!.Value}\\{keyPath}' AND ValueName='{value}'",
        };
        var query = new WqlEventQuery(queryString);
        var scope = new ManagementScope(@"\\.\root\default");
        _eventWatcher = new ManagementEventWatcher(scope, query);
        WeakEventManager<ManagementEventWatcher, EventArrivedEventArgs>.AddHandler(_eventWatcher, nameof(_eventWatcher.EventArrived), KeyWatcherOnEventArrived);
        try
        {
            _eventWatcher.Start();
            SendData();
        }
        catch (Exception)
        {
            Global.logger.Error("Registry not available, Query: {Query}", queryString);
        }
    }

    public void StopWatching()
    {
        if (_eventWatcher == null)
        {
            return;
        }

        WeakEventManager<ManagementEventWatcher, EventArrivedEventArgs>.RemoveHandler(_eventWatcher, nameof(_eventWatcher.EventArrived), KeyWatcherOnEventArrived);
        _eventWatcher.Stop();
        _eventWatcher.Dispose();
        _eventWatcher = null;
    }

    private void KeyWatcherOnEventArrived(object? sender, EventArrivedEventArgs e)
    {
        SendData();
    }

    private void SendData()
    {
        var localMachine = registryHive switch
        {
            RegistryHiveOpt.LocalMachine => Registry.LocalMachine,
            RegistryHiveOpt.CurrentUser => Registry.CurrentUser,
        };
        using var key1 = localMachine.OpenSubKey(key);
        var data = key1?.GetValue(value);
        if (data == null)
        {
            return;
        }

        RegistryChanged?.Invoke(this, new RegistryChangedEventArgs(data));
    }

    public void Dispose()
    {
        StopWatching();
    }
}