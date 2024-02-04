using System;
using System.Windows;
using Aurora.Utils;

namespace Aurora.Settings.Controls;

/// <summary>
/// Interaction logic for Window_VariableRegistryEditor.xaml
/// </summary>
public sealed partial class Window_VariableRegistryEditor : IDisposable
{
    private readonly TransparencyComponent _transparencyComponent;

    public Window_VariableRegistryEditor()
    {
        InitializeComponent();

        _transparencyComponent = new TransparencyComponent(this, null);
    }

    public void Dispose()
    {
        _transparencyComponent.Dispose();
    }

    private async void Window_VariableRegistryEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
        var deviceConfig = await ConfigManager.LoadDeviceConfig();
        VarRegistryEditor.VarRegistrySource = deviceConfig.VarRegistry;
    }

    private void Window_VariableRegistryEditor_OnUnloaded(object sender, RoutedEventArgs e)
    {
        Dispose();
    }
}