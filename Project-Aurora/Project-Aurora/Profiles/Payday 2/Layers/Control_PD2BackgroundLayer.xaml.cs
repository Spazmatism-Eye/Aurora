using System.Windows;
using System.Windows.Controls;

namespace AuroraRgb.Profiles.Payday_2.Layers
{
    /// <summary>
    /// Interaction logic for Control_CSGOBackgroundLayer.xaml
    /// </summary>
    public partial class Control_PD2BackgroundLayer : UserControl
    {
        private bool settingsset = false;
        private bool profileset = false;

        public Control_PD2BackgroundLayer()
        {
            InitializeComponent();
        }

        public Control_PD2BackgroundLayer(PD2BackgroundLayerHandler datacontext)
        {
            this.DataContext = datacontext.Properties;
            InitializeComponent();
        }

        private void UserControl_Loaded(object? sender, RoutedEventArgs e)
        {

            this.Loaded -= UserControl_Loaded;
        }

        private void sldAssaultSpeed_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.lblAssaultSpeed.Content = $"x {sldAssaultSpeed.Value.ToString("0.00")}";
        }
    }
}
