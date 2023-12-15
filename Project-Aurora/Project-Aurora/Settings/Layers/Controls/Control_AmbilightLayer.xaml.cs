using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Aurora.Settings.Layers.Controls;

/// <summary>
/// Interaction logic for Control_AmbilightLayer.xaml
/// </summary>
public partial class Control_AmbilightLayer
{
    private bool _settingsSet;

    public Control_AmbilightLayer()
    {
        InitializeComponent();
    }

    public Control_AmbilightLayer(AmbilightLayerHandler dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;
    }

    private void SetSettings()
    {
        if (DataContext is not AmbilightLayerHandler || _settingsSet) return;
        affectedKeys.Sequence = ((AmbilightLayerHandler)DataContext).Properties._Sequence;
        var properties = ((AmbilightLayerHandler)DataContext).Properties;
        XCoordinate.Value = properties.Coordinates.Left;
        YCoordinate.Value = properties.Coordinates.Top;
        HeightCoordinate.Value = properties.Coordinates.Height;
        WidthCoordinate.Value = properties.Coordinates.Width;
        _settingsSet = true;
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        SetSettings();
        Loaded -= UserControl_Loaded;
    }

    private void Coordinate_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (!_settingsSet)
            return;

        ((AmbilightLayerHandler)DataContext).Properties.Coordinates = new Rectangle(
            XCoordinate.Value ?? 0, 
            YCoordinate.Value ?? 0,
            WidthCoordinate.Value ?? 0,
            HeightCoordinate.Value ?? 0
        );
    }

    private void TextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        (DataContext as AmbilightLayerHandler)?.UpdateSpecificProcessHandle(((TextBox)e.Source).Text);
    }

    private void KeySequence_keys_SequenceUpdated(object? sender, RoutedPropertyChangedEventArgs<KeySequence> e)
    {
        if (IsLoaded && _settingsSet && DataContext is AmbilightLayerHandler)
        {
            ((AmbilightLayerHandler)DataContext).Properties._Sequence = e.NewValue;
        }
    }
}

public class AmbilightCaptureTypeValueConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is AmbilightCaptureType v && Enum.TryParse(parameter?.ToString(), out AmbilightCaptureType r) && v == r;
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class AmbilightTypeValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is AmbilightType v && Enum.TryParse(parameter?.ToString(), out AmbilightType r) && v == r;
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}