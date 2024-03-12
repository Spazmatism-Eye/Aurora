using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AuroraRgb.Profiles.LeagueOfLegends.GSI.Nodes;
using AuroraRgb.Utils;
using Xceed.Wpf.Toolkit;

namespace AuroraRgb.Profiles.LeagueOfLegends.Layers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LoLBackgroundLayer : UserControl
    {
        protected LoLBackgroundLayerHandler Context => DataContext as LoLBackgroundLayerHandler;

        private Champion selectedChampion;

        public LoLBackgroundLayer()
        {
            InitializeComponent();
        }

        public LoLBackgroundLayer(LoLBackgroundLayerHandler context)
        {
            InitializeComponent();

            this.DataContext = context;
        }

        private void UserControl_Loaded(object? sender, RoutedEventArgs e)
        {
            SetSettings();

            this.Loaded -= UserControl_Loaded;
        }

        private void SetSettings()
        {
            this.championPicker.SelectedItem = Champion.None;
        }

        private void championPicker_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox))
                return;

            selectedChampion = (Champion)((sender as ComboBox).SelectedItem);

            colorPicker.SelectedColor = Context.Properties.ChampionColors[selectedChampion].ToMediaColor();
        }

        private void colorPicker_SelectedColorChanged(object? sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!(sender is ColorPicker))
                return;

            Context.Properties.ChampionColors[selectedChampion] = ((sender as ColorPicker).SelectedColor ?? Color.FromArgb(0,0,0,0)).ToDrawingColor();
        }
    }
}
