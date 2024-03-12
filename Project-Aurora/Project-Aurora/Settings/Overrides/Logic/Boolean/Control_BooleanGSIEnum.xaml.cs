using System;
using System.Windows;
using AuroraRgb.Controls;
using AuroraRgb.Utils;

namespace AuroraRgb.Settings.Overrides.Logic;

public partial class Control_BooleanGSIEnum
{

    public Control_BooleanGSIEnum(BooleanGSIEnum context) {
        InitializeComponent();
        ((FrameworkElement)Content).DataContext = context;
    }

    /// <summary>Updates the enum value dropdown with a list of enum values for the current application and selected variable path.</summary>
    private void UpdateEnumDropDown() {
        Type selectedEnumType = null;
        var application = AttachedApplication.GetApplication(this);
        var isValid = ((FrameworkElement)Content).DataContext is BooleanGSIEnum ctx
                      && !string.IsNullOrWhiteSpace(ctx.VariablePath.GsiPath) // If the path to the enum GSI isn't empty
                      && application?.ParameterLookup != null // If the application parameter lookup is ready (and application isn't null)
                      && application.ParameterLookup.IsValidParameter(ctx.VariablePath.GsiPath) // If the param lookup has the specified GSI key
                      && (selectedEnumType = application.ParameterLookup[ctx.VariablePath.GsiPath].ClrType).IsEnum; // And the GSI variable is an enum type

        EnumVal.IsEnabled = isValid;
        EnumVal.ItemsSource = isValid ? EnumUtils.GetEnumItemsSource(selectedEnumType) : null;
    }

    // We don't do UpdateEnumDropDown in the constructor because it won't have been added to the visual tree at the point and therefore
    // the attached application property won't be set. If we wait til the control has added to the tree, the property is set.
    private void UserControl_Loaded(object? sender, RoutedEventArgs e) {
        UpdateEnumDropDown();
    }

    // Update the enum dropdown when the user selects a different enum path
    private void GameStateParameterPicker_SelectedPathChanged(object? sender, SelectedPathChangedEventArgs e) {
        UpdateEnumDropDown();
    }
}