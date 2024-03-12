using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AuroraRgb.Settings;
using AuroraRgb.Utils;

namespace AuroraRgb.Profiles.ETS2.Layers {
    /// <summary>
    /// Interaction logic for Control_ETS2BlinkerLayer.xaml
    /// </summary>
    public partial class Control_ETS2BlinkerLayer : UserControl {

        private bool settingsset = false;

        public Control_ETS2BlinkerLayer() {
            InitializeComponent();
        }

        public Control_ETS2BlinkerLayer(ETS2BlinkerLayerHandler datacontext) {
            InitializeComponent();
            DataContext = datacontext;
        }

        private ETS2BlinkerLayerHandler context => (ETS2BlinkerLayerHandler)DataContext;

        public void SetSettings() {
            if (DataContext is ETS2BlinkerLayerHandler && !settingsset) {
                ColorPicker_BlinkerOn.SelectedColor = ColorUtils.DrawingColorToMediaColor(context.Properties._BlinkerOnColor ?? System.Drawing.Color.Empty);
                ColorPicker_BlinkerOff.SelectedColor = ColorUtils.DrawingColorToMediaColor(context.Properties._BlinkerOffColor ?? System.Drawing.Color.Empty);
                LeftBlinker_keys.Sequence = context.Properties._LeftBlinkerSequence;
                RightBlinker_keys.Sequence = context.Properties._RightBlinkerSequence;
                settingsset = true;
            }
        }

        private void UserControl_Loaded(object? sender, RoutedEventArgs e) {
            SetSettings();
            Loaded -= UserControl_Loaded;
        }

        private void ColorPicker_BlinkerOn_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (IsLoaded && settingsset && DataContext is ETS2BlinkerLayerHandler && sender is Xceed.Wpf.Toolkit.ColorPicker && (sender as Xceed.Wpf.Toolkit.ColorPicker).SelectedColor.HasValue)
                context.Properties._BlinkerOnColor = ColorUtils.MediaColorToDrawingColor((sender as Xceed.Wpf.Toolkit.ColorPicker).SelectedColor.Value);
        }

        private void ColorPicker_BlinkerOff_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (IsLoaded && settingsset && DataContext is ETS2BlinkerLayerHandler && sender is Xceed.Wpf.Toolkit.ColorPicker && (sender as Xceed.Wpf.Toolkit.ColorPicker).SelectedColor.HasValue)
                context.Properties._BlinkerOffColor = ColorUtils.MediaColorToDrawingColor((sender as Xceed.Wpf.Toolkit.ColorPicker).SelectedColor.Value);
        }

        private void LeftBlinker_keys_SequenceUpdated(object? sender, RoutedPropertyChangedEventArgs<KeySequence> e) {
            if (IsLoaded && settingsset && DataContext is ETS2BlinkerLayerHandler)
                context.Properties._LeftBlinkerSequence = e.NewValue;
        }

        private void RightBlinker_keys_SequenceUpdated(object? sender, RoutedPropertyChangedEventArgs<KeySequence> e) {
            if (IsLoaded && settingsset && DataContext is ETS2BlinkerLayerHandler)
                context.Properties._RightBlinkerSequence = e.NewValue;
        }
    }
}
