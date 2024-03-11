using System;
using System.Windows;
using System.Windows.Controls;

namespace Aurora.Settings.Layers.Controls;

/// <summary>
/// Interaction logic for Control_PercentGradientLayer.xaml
/// </summary>
public partial class Control_PercentGradientLayer : IProfileContainingControl
{
    private bool _settingsSet;

    public Control_PercentGradientLayer()
    {
        InitializeComponent();
    }

    public Control_PercentGradientLayer(PercentGradientLayerHandler dataContext)
    {
        InitializeComponent();

        DataContext = dataContext;
    }

    private void SetSettings()
    {
        if (DataContext is not PercentGradientLayerHandler || _settingsSet) return;
        ComboBox_effect_type.SelectedValue = ((PercentGradientLayerHandler)DataContext).Properties.PercentType;
        updown_blink_value.Value = (int)(((PercentGradientLayerHandler)DataContext).Properties.BlinkThreshold * 100);
        CheckBox_threshold_reverse.IsChecked = ((PercentGradientLayerHandler)DataContext).Properties.BlinkDirection;
        KeySequence_keys.Sequence = ((PercentGradientLayerHandler)DataContext).Properties._Sequence;

        var brush = ((PercentGradientLayerHandler)DataContext).Properties.Gradient.GetMediaBrush();
        try
        {
            gradient_editor.Brush = brush;
        }
        catch (Exception exc)
        {
            Global.logger.Error(exc, "Could not set brush");
        }

        _settingsSet = true;
    }

    public void SetProfile(Profiles.Application profile)
    {
        VariablePath.Application = MaxVariablePath.Application = profile;
    }

    private void KeySequence_keys_SequenceUpdated(object? sender, RoutedPropertyChangedEventArgs<KeySequence> e)
    {
        if (IsLoaded && _settingsSet && DataContext is PercentGradientLayerHandler)
            ((PercentGradientLayerHandler)DataContext).Properties._Sequence = e.NewValue;
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        SetSettings();

        Loaded -= UserControl_Loaded;
    }

    private void Gradient_editor_BrushChanged(object? sender, ColorBox.BrushChangedEventArgs e)
    {
        if (IsLoaded && _settingsSet && DataContext is PercentGradientLayerHandler && sender is ColorBox.ColorBox colorBox)
            ((PercentGradientLayerHandler)DataContext).Properties.Gradient = new EffectsEngine.EffectBrush(colorBox.Brush);
    }

    private void updown_blink_value_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (IsLoaded && _settingsSet && DataContext is PercentGradientLayerHandler && sender is Xceed.Wpf.Toolkit.IntegerUpDown down && down.Value.HasValue)
            ((PercentGradientLayerHandler)DataContext).Properties.BlinkThreshold = down.Value.Value / 100.0D;
    }

    private void ComboBox_effect_type_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded && _settingsSet && DataContext is PercentGradientLayerHandler && sender is ComboBox comboBox)
        {
            ((PercentGradientLayerHandler)DataContext).Properties.PercentType = (PercentEffectType)comboBox.SelectedValue;
        }
    }

    private void CheckBox_threshold_reverse_Checked(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded && _settingsSet && DataContext is PercentGradientLayerHandler && sender is CheckBox checkBox && checkBox.IsChecked.HasValue)
        {
            ((PercentGradientLayerHandler)DataContext).Properties.BlinkDirection = checkBox.IsChecked.Value;
        }
    }
}