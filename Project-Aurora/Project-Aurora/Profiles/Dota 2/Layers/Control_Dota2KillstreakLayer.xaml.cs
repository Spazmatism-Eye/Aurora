using System.Windows;
using System.Windows.Media;
using Aurora.Utils;
using Xceed.Wpf.Toolkit;

namespace Aurora.Profiles.Dota_2.Layers;

/// <summary>
/// Interaction logic for Control_Dota2KillstreakLayer.xaml
/// </summary>
public partial class Control_Dota2KillstreakLayer
{
    private bool _settingsSet;

    public Control_Dota2KillstreakLayer()
    {
        InitializeComponent();
    }

    public Control_Dota2KillstreakLayer(Dota2KillstreakLayerHandler dataContext)
    {
        InitializeComponent();

        DataContext = dataContext;
    }

    private void SetSettings()
    {
        if (DataContext is not Dota2KillstreakLayerHandler || _settingsSet) return;
        ColorPicker_doublekill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._DoubleKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_triplekill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._TripleKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_quadkill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._QuadKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_pentakill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._PentaKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_hexakill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._HexaKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_septakill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._SeptaKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_octakill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._OctaKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_nonakill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._NonaKillstreakColor ?? System.Drawing.Color.Empty);
        ColorPicker_decakill.SelectedColor = ColorUtils.DrawingColorToMediaColor(((Dota2KillstreakLayerHandler)DataContext).Properties._DecaKillstreakColor ?? System.Drawing.Color.Empty);

        _settingsSet = true;
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        SetSettings();

        Loaded -= UserControl_Loaded;
    }

    private void ColorPicker_doublekill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._DoubleKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_triplekill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._TripleKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_quadkill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._QuadKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_pentakill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._PentaKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_hexakill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._HexaKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_septakill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._SeptaKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_octakill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._OctaKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_nonakill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._NonaKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }

    private void ColorPicker_decakill_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (IsLoaded && _settingsSet && DataContext is Dota2KillstreakLayerHandler && sender is ColorPicker { SelectedColor: not null } picker)
            ((Dota2KillstreakLayerHandler)DataContext).Properties._DecaKillstreakColor = ColorUtils.MediaColorToDrawingColor(picker.SelectedColor.Value);
    }
}