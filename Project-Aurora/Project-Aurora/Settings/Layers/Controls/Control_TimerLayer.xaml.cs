using System.Windows.Controls;
using AuroraRgb.Controls;

namespace AuroraRgb.Settings.Layers.Controls {

    public partial class Control_TimerLayer : UserControl {

        public Control_TimerLayer(TimerLayerHandler context) {
            InitializeComponent();
            DataContext = context;

            triggerKeyList.Keybinds = context.Properties._TriggerKeys;
        }

        private void triggerKeyList_KeybindsChanged(object? sender) {
            if (sender is KeyBindList kbl)
                (DataContext as TimerLayerHandler).Properties._TriggerKeys = kbl.Keybinds;
        }
    }
}
