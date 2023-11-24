﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using Aurora.Settings.Layers;

namespace Aurora.Settings.Controls;

/// <summary>
/// Interaction logic for Control_LayerControlPresenter.xaml
/// </summary>
public partial class Control_LayerControlPresenter
{
    private bool isSettingNewLayer;

    protected Layer _Layer;

    public Layer Layer { get => _Layer;
        set { _Layer = value; SetLayer(value); } }

    public Control_LayerControlPresenter()
    {
        InitializeComponent();
    }

    public Control_LayerControlPresenter(Layer layer) : this()
    {
        Layer = layer;
        grdLayerConfigs.Visibility = Visibility.Hidden;
        grd_LayerControl.IsHitTestVisible = true;
        grd_LayerControl.Effect = null;
    }

    private void SetLayer(Layer layer)
    {
        isSettingNewLayer = true;

        DataContext = layer;

        cmbLayerType.ItemsSource = layer.AssociatedApplication.AllowedLayers.OrderBy(l => l.Order).ThenBy(l => l.Name);
        cmbLayerType.SelectedValue = Layer.Handler.GetType();

        ctrlLayerTypeConfig.Content = layer.Control;
        chkLayerSmoothing.IsChecked = Layer.Handler.EnableSmoothing;
        chk_ExcludeMask.IsChecked = Layer.Handler._EnableExclusionMask ?? false;
        keyseq_ExcludeMask.Sequence = Layer.Handler._ExclusionMask;
        sldr_Opacity.Value = (Layer.Handler._Opacity ?? 1d) * 100.0;
        lbl_Opacity_Text.Text = $"{(int)sldr_Opacity.Value} %";

        grdLayerConfigs.Visibility = Visibility.Hidden;
        overridesEditor.Visibility = Visibility.Hidden;
        btnConfig.Visibility = Visibility.Visible;
        btnOverrides.Visibility = Visibility.Visible;
        grd_LayerControl.IsHitTestVisible = true;
        grd_LayerControl.Effect = null;
        isSettingNewLayer = false;

        overridesEditor.Layer = layer;
    }

    private void cmbLayerType_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is ComboBox)
        {
            _Layer?.Dispose();
            ResetLayer((Type)(sender as ComboBox).SelectedValue);
        }
    }

    private void ResetLayer(Type type)
    {
        if (IsLoaded && !isSettingNewLayer && type != null)
        {
            _Layer.Handler = (ILayerHandler)Activator.CreateInstance(type);

            ctrlLayerTypeConfig.Content = _Layer.Control;
            chkLayerSmoothing.IsChecked = _Layer.Handler.EnableSmoothing;
            chk_ExcludeMask.IsChecked = Layer.Handler._EnableExclusionMask ?? false;
            keyseq_ExcludeMask.Sequence = Layer.Handler._ExclusionMask;
            sldr_Opacity.Value = (int)(Layer.Handler.Opacity * 100.0f);
            lbl_Opacity_Text.Text = $"{(int)sldr_Opacity.Value} %";
            _Layer.AssociatedApplication.SaveProfiles();

            overridesEditor.ForcePropertyListUpdate();
        }
    }

    private void btnReset_Click(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is Button)
        {
            ResetLayer((Type)cmbLayerType.SelectedValue);
        }
    }

    private void btnConfig_Click(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is Button)
        {
            var v = grdLayerConfigs.IsVisible;
            grdLayerConfigs.Visibility = v ? Visibility.Hidden : Visibility.Visible;
            grd_LayerControl.IsHitTestVisible = v;
            grd_LayerControl.Effect = v ? null : new BlurEffect();
            btnOverrides.Visibility = v ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private void chkLayerSmoothing_Checked(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is CheckBox checkBox)
            Layer.Handler.EnableSmoothing = checkBox.IsChecked.Value;
    }

    private void chk_ExcludeMask_Checked(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is CheckBox checkBox)
            Layer.Handler._EnableExclusionMask = checkBox.IsChecked.Value;

        //keyseq_ExcludeMask.IsEnabled = Layer.Handler.EnableExclusionMask;
    }

    private void keyseq_ExcludeMask_SequenceUpdated(object? sender, EventArgs e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is KeySequence sequence)
            Layer.Handler._ExclusionMask = sequence;
    }

    private void sldr_Opacity_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (IsLoaded && !isSettingNewLayer && sender is Slider slider)
        {
            Layer.Handler._Opacity = (float)slider.Value / 100.0f;
            lbl_Opacity_Text.Text = $"{(int)slider.Value} %";
        }
    }

    private void btnOverrides_Click(object? sender, RoutedEventArgs e) {
        if (IsLoaded && !isSettingNewLayer) {
            var v = overridesEditor.IsVisible;
            overridesEditor.Visibility = v ? Visibility.Hidden : Visibility.Visible;
            grd_LayerControl.IsHitTestVisible = v;
            btnConfig.Visibility = v ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}