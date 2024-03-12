using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;

namespace AuroraRgb.Profiles.XCOM;

public class XCOM : Application
{
    public XCOM()
        : base(new LightEventConfig {
            Name = "XCOM: Enemy Unknown",
            ID = "XCOM", ProcessNames = new[] { "xcomgame.exe" },
            SettingsType = typeof(FirstTimeApplicationSettings),
            ProfileType = typeof(XCOMProfile),
            OverviewControlType = typeof(Control_XCOM),
            GameStateType = typeof(GameState_Wrapper),
            IconURI = "Resources/xcom_64x64.png"
        })
    {
        AllowLayer<WrapperLightsLayerHandler>();
    }
}