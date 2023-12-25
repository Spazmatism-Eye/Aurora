using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Common.Devices;

namespace Aurora.Controls;

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
        get => GetValue(VarRegistrySourceProperty) as VariableRegistry ?? Global.DeviceConfiguration.VarRegistry;
        set => SetValue(VarRegistrySourceProperty, value);
    }

    private void UpdateControls()
    {
        stack_Options.Children.Clear();
        if (RegisteredVariables == null)
            return;

        foreach (var variableName in RegisteredVariables.GetRegisteredVariableKeys())
        {
            var varItem = new Control_VariableRegistryItem {
                VariableName = variableName,
                VarRegistry = VarRegistrySource,
            };

            stack_Options.Children.Add(varItem);
            stack_Options.Children.Add(new Separator { Height = 5, Opacity = 0 });
        }
    }

    private static void VarRegistrySourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var self = (Control_VariableRegistryEditor)obj;

        if (e.OldValue == e.NewValue || self.stack_Options.Children.Count == 0)
            return;

        var newRegistry = (VariableRegistry)e.NewValue;
        foreach (UIElement child in self.stack_Options.Children)
        {
            if (child is Control_VariableRegistryItem item)
                item.VarRegistry = newRegistry;
        }
    }

    public Control_VariableRegistryEditor()
    {
        InitializeComponent();
    }
}