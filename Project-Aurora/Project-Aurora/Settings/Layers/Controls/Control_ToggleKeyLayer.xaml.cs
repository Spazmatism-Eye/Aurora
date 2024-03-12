using System.Windows.Controls;
using AuroraRgb.Controls;

namespace AuroraRgb.Settings.Layers.Controls {

    public partial class Control_ToggleKeyLayer : UserControl {

        public Control_ToggleKeyLayer(ToggleKeyLayerHandler context) {
            InitializeComponent();
            DataContext = context;

            triggerKeyList.Keybinds = context.Properties.TriggerKeys;
        }

        private void triggerKeyList_KeybindsChanged(object? sender) {
            if (IsLoaded && DataContext is ToggleKeyLayerHandler ctx && sender is KeyBindList kbl)
                ctx.Properties.TriggerKeys = kbl.Keybinds;
        }
    }
}
