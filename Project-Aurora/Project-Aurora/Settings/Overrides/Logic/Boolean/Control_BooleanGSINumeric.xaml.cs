using System.Windows.Controls;
using AuroraRgb.Settings.Overrides.Logic.Number;
using AuroraRgb.Utils;

namespace AuroraRgb.Settings.Overrides.Logic {
    /// <summary>
    /// Interaction logic for Control_ConditionGSINumeric.xaml
    /// </summary>
    public partial class Control_ConditionGSINumeric : UserControl {

        public Control_ConditionGSINumeric(BooleanGSINumeric context) {
            InitializeComponent();
            DataContext = context;
            OperatorCb.ItemsSource = EnumUtils.GetEnumItemsSource<ComparisonOperator>();
        }
    }
}
