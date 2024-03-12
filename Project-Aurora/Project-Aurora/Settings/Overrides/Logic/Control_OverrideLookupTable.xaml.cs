using System.Windows;
using System.Windows.Controls;

namespace AuroraRgb.Settings.Overrides.Logic {
    /// <summary>
    /// Interaction logic for Control_OverrideLookupTable.xaml
    /// </summary>
    public partial class Control_OverrideLookupTable : UserControl {

        public OverrideLookupTable Table { get; }

        public Control_OverrideLookupTable(OverrideLookupTable context) {
            InitializeComponent();
            Table = context;
            DataContext = this;
        }

        private void AddNewLookup_Click(object? sender, RoutedEventArgs e) {
            Table?.CreateNewLookup();
        }

        private void DeleteLookupEntry_Click(object? sender, RoutedEventArgs e) {
            var dc = (OverrideLookupTable.LookupTableEntry)((Button)sender).DataContext;
            Table.LookupTable.Remove(dc);
        }
    }
}