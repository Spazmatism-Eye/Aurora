using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Aurora.Profiles.LethalCompany.GSI;
using Application = Aurora.Profiles.Application;

namespace AuroraRgb.Profiles.LethalCompany {
    /// <summary>
    /// Interaction logic for Control_LethalCompany.xaml
    /// </summary>
    public partial class Control_LethalCompany : UserControl
    {

        private Application profile;

        public Control_LethalCompany(Application profile)
        {
            this.profile = profile;

            InitializeComponent();
            SetSettings();

            profile.ProfileChanged += (sender, e) => SetSettings();
        }

        private void SetSettings()
        {
            GameEnabled.IsChecked = profile.Settings.IsEnabled;
        }

        #region Overview handlers
        private void GameEnabled_Checked(object? sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                profile.Settings.IsEnabled = GameEnabled.IsChecked ?? false;
                profile.SaveProfiles();
            }
        }

        private void GoToQModManagerPage_Click(object? sender, RoutedEventArgs e)
        {
            Process.Start("explorer", @"https://www.nexusmods.com/LethalCompany/mods/16/");
        }

        private void GoToModDownloadPage_Click(object? sender, RoutedEventArgs e)
        {
            Process.Start("explorer", @"https://www.nexusmods.com/LethalCompany/mods/171");
        }

        #endregion
        
        #region Preview Handlers
        private GameState_LethalCompany State => profile.Config.Event._game_state as GameState_LethalCompany;

        private void InGameCh_Checked(object? sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                State.GameState.GameState = 2;
                State.GameState.InGame = true;
            }
            else
            {
                State.GameState.GameState = 0;
                State.GameState.InGame = false;
            }
        }

        private void HealthSlider_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
        {
            State.Player.Health = (int)e.NewValue;
            
        }

        private void StaminaSlider_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
        {
            State.Player.Stamina = (int)e.NewValue;
        }
        #endregion
    }
}