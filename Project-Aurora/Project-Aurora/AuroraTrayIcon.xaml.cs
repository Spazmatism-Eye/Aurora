using System;
using System.Windows;

namespace AuroraRgb;

public sealed partial class AuroraTrayIcon : IDisposable
{
    private readonly AuroraControlInterface _auroraControlInterface;

    public event EventHandler? DisplayWindow;

    public AuroraTrayIcon(AuroraControlInterface auroraControlInterface)
    {
        _auroraControlInterface = auroraControlInterface;
        InitializeComponent();
    }
    
    private async void trayicon_menu_quit_Click(object? sender, RoutedEventArgs e)
    {
        await _auroraControlInterface.ShutdownDevices();
        _auroraControlInterface.ExitApp();
    }

    private void trayicon_menu_settings_Click(object? sender, RoutedEventArgs e)
    {
        DisplayWindow?.Invoke(this, EventArgs.Empty);
    }

    private void trayicon_menu_restart_aurora_Click(object? sender, RoutedEventArgs e)
    {
        _auroraControlInterface.RestartAurora();
    }

    private async void trayicon_menu_restart_devices_Click(object? sender, RoutedEventArgs e)
    {
        await _auroraControlInterface.RestartDevices();
    }

    private void trayicon_menu_quit_aurora_Click(object? sender, RoutedEventArgs e)
    {
        _auroraControlInterface.ExitApp();
    }

    private async void trayicon_menu_quit_devices_Click(object? sender, RoutedEventArgs e)
    {
        await _auroraControlInterface.ShutdownDevices();
    }

    private void trayicon_TrayMouseDoubleClick(object? sender, RoutedEventArgs e)
    {
        DisplayWindow?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        TrayIcon.Dispose();
    }
}