using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Common.Devices;

namespace AuroraRgb.Controls;

/// <summary>
/// Interaction logic for Window_VariableRegistryEditor.xaml
/// </summary>
public partial class Control_VariableRegistryEditor
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public static readonly DependencyProperty RegisteredVariablesProperty = DependencyProperty.Register(nameof(RegisteredVariables), typeof(VariableRegistry), typeof(Control_VariableRegistryEditor));

    public VariableRegistry? RegisteredVariables
    {
        get => GetValue(RegisteredVariablesProperty) as VariableRegistry;
        set
        {
            SetValue(RegisteredVariablesProperty, value);
            UpdateControls();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public static readonly DependencyProperty VarRegistrySourceProperty = DependencyProperty.Register(nameof(VarRegistrySource), typeof(VariableRegistry), typeof(Control_VariableRegistryEditor),
        new PropertyMetadata { PropertyChangedCallback = VarRegistrySourceChanged });

    public VariableRegistry VarRegistrySource
    {
        get => (VariableRegistry)GetValue(VarRegistrySourceProperty);
        set => SetValue(VarRegistrySourceProperty, value);
    }

    public Control_VariableRegistryEditor()
    {
        InitializeComponent();
    }

    private void UpdateControls()
    {
        StackOptions.Children.Clear();
        if (RegisteredVariables == null)
            return;

        foreach (var variableName in RegisteredVariables.GetRegisteredVariableKeys())
        {
            var varItem = new Control_VariableRegistryItem {
                VariableName = variableName,
                VarRegistry = VarRegistrySource,
            };

            StackOptions.Children.Add(varItem);
            StackOptions.Children.Add(new Separator { Height = 5, Opacity = 0 });
        }
    }

    private static void VarRegistrySourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var self = (Control_VariableRegistryEditor)obj;

        if (e.OldValue == e.NewValue || self.StackOptions.Children.Count == 0)
            return;

        var newRegistry = (VariableRegistry)e.NewValue;
        foreach (UIElement child in self.StackOptions.Children)
        {
            if (child is Control_VariableRegistryItem item)
                item.VarRegistry = newRegistry;
        }
    }
}