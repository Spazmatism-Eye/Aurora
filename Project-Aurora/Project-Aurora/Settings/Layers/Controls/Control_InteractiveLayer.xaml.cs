using System.Windows.Controls;

namespace AuroraRgb.Settings.Layers.Controls {

    public partial class Control_InteractiveLayer : UserControl {

        public Control_InteractiveLayer(InteractiveLayerHandler context) {
            InitializeComponent();
            DataContext = context;
        }
    }
}
