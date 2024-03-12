using AuroraRgb.Settings;

namespace AuroraRgb.Profiles.Generic_Application
{
    public class GenericApplicationSettings : ApplicationSettings
    {
        public string ApplicationName { get; set; } = "New Application Profile";

        public GenericApplicationSettings()
        {

        }

        public GenericApplicationSettings(string appname) : base()
        {
            ApplicationName = appname;
        }
    }
}
