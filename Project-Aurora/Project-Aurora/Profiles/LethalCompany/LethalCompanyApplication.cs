using Aurora.Profiles;
using AuroraRgb.Profiles.LethalCompany;

namespace Aurora.Profiles.LethalCompany;

public class LethalCompany : Application {

    public LethalCompany() : base(new LightEventConfig
    {
        Name = "Lethal Company",
        ID = "lethalcompany",
        AppID = "1966720",
        ProcessNames = new[] { "Lethal Company.exe" },
        ProfileType = typeof(LethalCompanyProfile),
        OverviewControlType = typeof(Control_LethalCompany),
        GameStateType = typeof(GSI.GameState_LethalCompany),
        IconURI = "Resources/LethalCompany.png"
    })
    { }        
}