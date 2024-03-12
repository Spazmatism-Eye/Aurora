using System.Windows.Controls;
using AuroraRgb.Settings;
using AuroraRgb.Utils;

namespace AuroraRgb.Controls {

    public partial class Control_GradientEditor : UserControl {

        public Control_GradientEditor(LayerEffectConfig gradient) {
            InitializeComponent();
            animTypeCb.ItemsSource = EnumUtils.GetEnumItemsSource<AnimationType>();
            DataContext = gradient;
        }
    }
}
