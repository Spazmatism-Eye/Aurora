using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AuroraRgb.EffectsEngine;
using ColorBox.Implementation;
using Xceed.Wpf.Toolkit;

namespace AuroraRgb.Settings.Layers.Controls;

/// <summary>
/// Interaction logic for Control_GradientLayer.xaml
/// </summary>
public partial class Control_GradientLayer
{
    private bool _settingsSet;

    public Control_GradientLayer()
    {
        InitializeComponent();
    }

    public Control_GradientLayer(GradientLayerHandler dataContext)
    {
        InitializeComponent();

        DataContext = dataContext;
    }

    private void SetSettings()
    {
        if (DataContext is not GradientLayerHandler || _settingsSet) return;
        wave_size_slider.Value = ((GradientLayerHandler)DataContext).Properties.GradientConfig.GradientSize;
        wave_size_label.Text = ((GradientLayerHandler)DataContext).Properties.GradientConfig.GradientSize + " %";
        effect_speed_slider.Value = ((GradientLayerHandler)DataContext).Properties.GradientConfig.Speed;
        effect_speed_label.Text = "x " + ((GradientLayerHandler)DataContext).Properties.GradientConfig.Speed;
        effect_angle.Text = ((GradientLayerHandler)DataContext).Properties.GradientConfig.Angle.ToString(CultureInfo.InvariantCulture);
        effect_animation_type.SelectedValue = ((GradientLayerHandler)DataContext).Properties.GradientConfig.AnimationType;
        effect_animation_reversed.IsChecked = ((GradientLayerHandler)DataContext).Properties.GradientConfig.AnimationReverse;
        var brush = ((GradientLayerHandler)DataContext).Properties.GradientConfig.Brush.GetMediaBrush();
        try
        {
            gradient_editor.Brush = brush;
        }
        catch (Exception exc)
        {
            Global.logger.Error(exc, "Could not set brush");
        }

        KeySequence_keys.Sequence = ((GradientLayerHandler)DataContext).Properties._Sequence;

        _settingsSet = true;
    }

    private void Gradient_editor_BrushChanged(object? sender, BrushChangedEventArgs e)
    {
        if (IsLoaded && _settingsSet && DataContext is GradientLayerHandler && sender is ColorBox.Implementation.ColorBox colorBox)
            ((GradientLayerHandler)DataContext).Properties.GradientConfig.Brush = new EffectBrush(colorBox.Brush);
    }

    private void Button_SetGradientRainbow_Click(object? sender, RoutedEventArgs e)
    {
        ((GradientLayerHandler)DataContext).Properties.GradientConfig.Brush = new EffectBrush(ColorSpectrum.Rainbow);

        var brush = ((GradientLayerHandler)DataContext).Properties.GradientConfig.Brush.GetMediaBrush();
        try
        {
            gradient_editor.Brush = brush;
        }
        catch (Exception exc)
        {
            Global.logger.Error(exc, "Could not set brush");
        }
    }

    private void Button_SetGradientRainbowLoop_Click(object? sender, RoutedEventArgs e)
    {
        ((GradientLayerHandler)DataContext).Properties.GradientConfig.Brush = new EffectBrush(ColorSpectrum.RainbowLoop);

        var brush = ((GradientLayerHandler)DataContext).Properties.GradientConfig.Brush.GetMediaBrush();
        try
        {
            gradient_editor.Brush = brush;
        }
        catch (Exception exc)
        {
            Global.logger.Error(exc, "Could not set brush");
        }
    }
    private void effect_speed_slider_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!IsLoaded || !_settingsSet || DataContext is not GradientLayerHandler || sender is not Slider slider) return;
        ((GradientLayerHandler)DataContext).Properties.GradientConfig.Speed = (float)slider.Value;

        if (effect_speed_label != null)
            effect_speed_label.Text = "x " + slider.Value;
    }

    private void wave_size_slider_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!IsLoaded || !_settingsSet || DataContext is not GradientLayerHandler || sender is not Slider) return;
        ((GradientLayerHandler)DataContext).Properties.GradientConfig.GradientSize = (float)e.NewValue;
                
        if (wave_size_label != null)
        {
            wave_size_label.Text = e.NewValue + " %";
            if (e.NewValue == 0)
            {
                wave_size_label.Text = "Stop";
            }
        }
        TriggerPropertyChanged();
    }

    private void effect_angle_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (!IsLoaded || !_settingsSet || DataContext is not GradientLayerHandler || sender is not IntegerUpDown integerUpDown) return;

        if (float.TryParse(integerUpDown.Text, out var outval))
        {
            integerUpDown.Background = new SolidColorBrush(Color.FromArgb(255, 24, 24, 24));

            ((GradientLayerHandler)DataContext).Properties.GradientConfig.Angle = outval;
        }
        else
        {
            integerUpDown.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            integerUpDown.ToolTip = "Entered value is not a number";
        }
        TriggerPropertyChanged();
    }

    private void effect_animation_type_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded || !_settingsSet || DataContext is not GradientLayerHandler || sender is not ComboBox comboBox) return;

        ((GradientLayerHandler)DataContext).Properties.GradientConfig.AnimationType = (AnimationType)comboBox.SelectedValue;
        TriggerPropertyChanged();
    }

    private void effect_animation_reversed_Checked(object? sender, RoutedEventArgs e)
    {
        if (!IsLoaded || !_settingsSet || DataContext is not GradientLayerHandler || sender is not CheckBox checkBox) return;

        ((GradientLayerHandler)DataContext).Properties.GradientConfig.AnimationReverse = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
        TriggerPropertyChanged();
    }

    private void KeySequence_keys_SequenceUpdated(object? sender, RoutedPropertyChangedEventArgs<KeySequence> e)
    {
        if (!IsLoaded || !_settingsSet || DataContext is not GradientLayerHandler) return;

        ((GradientLayerHandler)DataContext).Properties._Sequence = e.NewValue;
        TriggerPropertyChanged();
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        SetSettings();

        Loaded -= UserControl_Loaded;
    }

    private void TriggerPropertyChanged()
    {
        var layerHandler = (GradientLayerHandler) DataContext;
        layerHandler.Properties.OnPropertiesChanged(this);
    }
}