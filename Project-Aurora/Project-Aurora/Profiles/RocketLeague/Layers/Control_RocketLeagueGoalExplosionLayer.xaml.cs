using System.Windows.Controls;

namespace AuroraRgb.Profiles.RocketLeague.Layers
{
    /// <summary>
    /// Interaction logic for Control_RocketLeagueBackgroundLayer.xaml
    /// </summary>
    public partial class Control_RocketLeagueGoalExplosionLayer : UserControl
    {
        public Control_RocketLeagueGoalExplosionLayer()
        {
            InitializeComponent();
        }

        public Control_RocketLeagueGoalExplosionLayer(RocketLeagueGoalExplosionLayerHandler datacontext)
        {
            InitializeComponent();
            this.DataContext = datacontext;
        }
    }
}
