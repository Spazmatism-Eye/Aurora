using System.Windows;
using System.Windows.Media;
using AuroraRgb.Settings;
using AuroraRgb.Utils;
using Xceed.Wpf.Toolkit;

namespace Plugin_Example.Layers;

/// <summary>
/// Interaction logic for Control_DefaultLayer.xaml
/// </summary>
public partial class Control_ExampleLayer
{
    private bool _settingsset;

    public Control_ExampleLayer()
    {
        InitializeComponent();
    }

    public Control_ExampleLayer(ExampleLayerHandler dataContext)
    {
        InitializeComponent();

        DataContext = dataContext;
    }

    public void SetSettings()
    {
        if(DataContext is ExampleLayerHandler layer && !_settingsset)
        {
            ColorPicker_primaryColor.SelectedColor = ColorUtils.DrawingColorToMediaColor(((ExampleLayerHandler)DataContext).Properties._PrimaryColor ?? System.Drawing.Color.Empty);
            KeySequence_keys.Sequence = layer.Properties._Sequence;

            _settingsset = true;
        }
    }

    private void ColorPicker_primaryColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsset && DataContext is ExampleLayerHandler layer && sender is ColorPicker)
            layer.Properties._PrimaryColor = ColorUtils.MediaColorToDrawingColor(e.NewValue.GetValueOrDefault());
    }

    private void KeySequence_keys_SequenceUpdated(object sender, RoutedPropertyChangedEventArgs<KeySequence> e)
    {
        if (IsLoaded && _settingsset && DataContext is ExampleLayerHandler layer)
            layer.Properties._Sequence = e.NewValue;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        SetSettings();

        Loaded -= UserControl_Loaded;
    }
}