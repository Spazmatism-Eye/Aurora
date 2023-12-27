using System.Windows;
using System.Windows.Controls;

namespace Aurora.Settings.Layers.Controls;

/// <summary>
/// Interaction logic for Control_ScriptLayer.xaml
/// </summary>
public partial class Control_ScriptLayer
{
    private Profiles.Application Application { get; set; }

    public Control_ScriptLayer()
    {
        InitializeComponent();
    }

    public Control_ScriptLayer(ScriptLayerHandler layerHandler) : this()
    {
        DataContext = layerHandler;
        SetProfile(layerHandler.Application);
        UpdateScriptSettings();
    }

    public void SetProfile(Profiles.Application application)
    {
        cboScripts.ItemsSource = application.EffectScripts.Keys;
        cboScripts.IsEnabled = application.EffectScripts.Keys.Count > 0;
        Application = application;
    }

    private void cboScripts_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var handler = (ScriptLayerHandler)DataContext;
        var script = (string)e.AddedItems[0]!;
        handler.Properties.Script = script;
        UpdateScriptSettings();
    }

    private void UpdateScriptSettings()
    {
        var handler = (ScriptLayerHandler)DataContext;
        ScriptPropertiesEditor.RegisteredVariables = handler.GetScriptPropertyRegistry();
        var varReg = ScriptPropertiesEditor.RegisteredVariables;
        ScriptPropertiesEditor.Visibility = varReg == null || varReg.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        if (handler.IsScriptValid)
            ScriptPropertiesEditor.VarRegistrySource = handler.Properties.ScriptProperties;
    }

    private void refreshScriptList_Click(object? sender, RoutedEventArgs e) {
        Application.ForceScriptReload();
        cboScripts.Items.Refresh();
        cboScripts.IsEnabled = Application.EffectScripts.Keys.Count > 0;
    }
}