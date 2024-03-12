using System.Diagnostics;
using System.Windows;

namespace AuroraRgb.Profiles.StardewValley;

public partial class Control_StardewValley
{
    private Application profile;

    public Control_StardewValley(Application profile)
    {
        this.profile = profile;

        InitializeComponent();
        SetSettings();

        profile.ProfileChanged += (_, _) => SetSettings();
    }

    private void SetSettings()
    {
        GameEnabled.IsChecked = profile.Settings.IsEnabled;
    }

    private void GameEnabled_Checked(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded)
        {
            profile.Settings.IsEnabled = GameEnabled.IsChecked ?? false;
            profile.SaveProfiles();
        }
    }

    private void GoToSMAPIPage_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start("explorer", @"https://www.nexusmods.com/stardewvalley/mods/2400");
    }

    private void GoToModDownloadPage_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start("explorer", @"https://www.nexusmods.com/stardewvalley/mods/6088");
    }
}