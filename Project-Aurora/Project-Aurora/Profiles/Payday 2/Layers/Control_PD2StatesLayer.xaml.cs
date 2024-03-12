using System.Windows;
using System.Windows.Controls;

namespace AuroraRgb.Profiles.Payday_2.Layers
{
    /// <summary>
    /// Interaction logic for Control_PD2StatesLayer.xaml
    /// </summary>
    public partial class Control_PD2StatesLayer : UserControl
    {
        private bool settingsset = false;
        private bool profileset = false;

        public Control_PD2StatesLayer()
        {
            InitializeComponent();
        }

        public Control_PD2StatesLayer(PD2StatesLayerHandler datacontext)
        {
            this.DataContext = datacontext.Properties;
            InitializeComponent();
        }

        private void UserControl_Loaded(object? sender, RoutedEventArgs e)
        {

            this.Loaded -= UserControl_Loaded;
        }

        private void sldSwanSongSpeed_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.lblSwanSongSpeed.Content = $"x {sldSwanSongSpeed.Value.ToString("0.00")}";
        }
    }
}
