using System;
using Aurora.Utils;

namespace Aurora.Settings.Controls;

/// <summary>
/// Interaction logic for Window_VariableRegistryEditor.xaml
/// </summary>
public partial class Window_VariableRegistryEditor : IDisposable
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
}