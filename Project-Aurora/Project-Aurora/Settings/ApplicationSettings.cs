using System.ComponentModel;

namespace AuroraRgb.Settings
{
    public class ApplicationSettings : INotifyPropertyChanged
    {
        public bool IsEnabled { get; set; } = true;
        public bool IsOverlayEnabled { get; set; } = true;
        public bool Hidden { get; set; } = false;
        public string SelectedProfile { get; set; } = "default";

        public event PropertyChangedEventHandler PropertyChanged;

        public ApplicationSettings() { }
    }

    public class FirstTimeApplicationSettings : ApplicationSettings
    {
        public bool IsFirstTimeInstalled { get; set; }

        public FirstTimeApplicationSettings() { }
    }
}
