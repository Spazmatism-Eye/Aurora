using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AuroraRgb.Utils;
using KeySequence = AuroraRgb.Settings.KeySequence;

namespace AuroraRgb.Profiles.ETS2.Layers {
    /// <summary>
    /// Interaction logic for Control_ETS2BeaconLayer.xaml
    /// </summary>
    public partial class Control_ETS2BeaconLayer : UserControl {

        private bool settingsset = false;

        public Control_ETS2BeaconLayer() {
            InitializeComponent();
        }

        public Control_ETS2BeaconLayer(ETS2BeaconLayerHandler datacontext) {
            DataContext = datacontext;
            InitializeComponent();
        }

        private ETS2BeaconLayerHandler context => (ETS2BeaconLayerHandler)DataContext;
        private bool isReady => IsLoaded && settingsset && DataContext is ETS2BeaconLayerHandler;

        public void SetSettings() {
            if (DataContext is ETS2BeaconLayerHandler && !settingsset) {
                lightMode.SelectedValue = context.Properties._BeaconStyle;
                speedSlider.Value = (double)context.Properties._Speed;
                speedSlider.IsEnabled = context.Properties._BeaconStyle == ETS2_BeaconStyle.Simple_Flash;
                beaconColorPicker.SelectedColor = ColorUtils.DrawingColorToMediaColor(context.Properties._PrimaryColor ?? System.Drawing.Color.Empty);
                keyPicker.Sequence = context.Properties._Sequence;
                settingsset = true;
            }
        }

        private void UserControl_Loaded(object? sender, RoutedEventArgs e) {
            SetSettings();
            /*lightMode.Items.Add(ETS2_BeaconStyle.Simple_Flash);
            lightMode.Items.Add(ETS2_BeaconStyle.Two_Half);
            lightMode.Items.Add(ETS2_BeaconStyle.Fancy_Flash);
            lightMode.Items.Add(ETS2_BeaconStyle.Flip_Flop);*/
            Loaded -= UserControl_Loaded;
        }

        private void lightMode_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
            if (isReady && sender is ComboBox comboBox) {
                context.Properties._BeaconStyle = (ETS2_BeaconStyle)comboBox.SelectedValue;
                speedSlider.IsEnabled = context.Properties._BeaconStyle == ETS2_BeaconStyle.Simple_Flash;
            }
        }

        private void speedSlider_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e) {
            if (isReady && sender is Slider slider)
                context.Properties._Speed = (float)slider.Value;
        }

        private void beaconColorPicker_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (isReady && sender is Xceed.Wpf.Toolkit.ColorPicker { SelectedColor: not null } colorPicker)
                context.Properties._PrimaryColor = ColorUtils.MediaColorToDrawingColor(colorPicker.SelectedColor.Value);
        }

        private void keyPicker_SequenceUpdated(object sender, RoutedPropertyChangedEventArgs<KeySequence> e) {
            if (isReady)
                context.Properties._Sequence = e.NewValue;
        }
    }
}
