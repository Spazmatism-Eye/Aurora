using System.Windows.Controls;
using AuroraRgb.Profiles;

namespace AuroraRgb.Settings.Layers.Controls {

    public partial class Control_BinaryCounterLayer : UserControl {

        public Control_BinaryCounterLayer(BinaryCounterLayerHandler context) {
            InitializeComponent();
            SetApplication(context.Application);
            DataContext = context;
        }

        public void SetApplication(Application app) {
            varPathPicker.Application = app;
        }
    }
}
