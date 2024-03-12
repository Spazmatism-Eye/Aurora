using System.Windows.Controls;

namespace AuroraRgb.Settings.Layers.Controls {

    public partial class Control_ToolbarLayer : UserControl {

        public Control_ToolbarLayer(ToolbarLayerHandler context) {
            InitializeComponent();
            DataContext = context;
        }
    }
}
