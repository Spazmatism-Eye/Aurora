using AuroraRgb.Settings;

namespace AuroraRgb.Profiles.Metro_Last_Light;

public class MetroLL : Application
{
    public MetroLL()
        : base(new LightEventConfig
        {
            Name = "Metro: Last Light",
            ID = "MetroLL",
            ProcessNames = new[] { "metroll.exe" },
            SettingsType = typeof(FirstTimeApplicationSettings),
            ProfileType = typeof(MetroLLProfile),
            OverviewControlType = typeof(Control_MetroLL),
            GameStateType = typeof(GameState_Wrapper),
            IconURI = "Resources/metro_ll_48x48.png"
        })
    {
    }
}