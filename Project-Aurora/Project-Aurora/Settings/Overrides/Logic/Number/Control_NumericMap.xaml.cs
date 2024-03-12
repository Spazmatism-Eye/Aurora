using System.Windows.Controls;

namespace AuroraRgb.Settings.Overrides.Logic.Number {

    public partial class Control_NumericMap : UserControl {

        public Control_NumericMap(NumberMap context) {
            InitializeComponent();
            DataContext = context;
        }
    }
}
