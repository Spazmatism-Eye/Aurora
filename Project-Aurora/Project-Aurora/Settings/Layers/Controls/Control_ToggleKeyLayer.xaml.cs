using System.Windows.Controls;
using Aurora.Controls;

namespace Aurora.Settings.Layers.Controls {

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
