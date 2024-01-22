using System.Windows;

namespace Aurora.Profiles.Desktop;

/// <summary>
/// Interaction logic for Control_Desktop.xaml
/// </summary>
public partial class Control_Desktop
{
    private readonly Application _profileManager;

    public Control_Desktop(Application profile)
    {
        InitializeComponent();

        _profileManager = profile;
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        profile_enabled.IsChecked = _profileManager.Settings.IsEnabled;
    }

    private void game_enabled_Checked(object? sender, RoutedEventArgs e)
    {
        if (!IsLoaded) return;
        _profileManager.Settings.IsEnabled = profile_enabled.IsChecked ?? false;
        _profileManager.SaveProfiles();
    }
}