using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;
using Common.Utils;

namespace AuroraRgb.Profiles.Logitech;

public class LogitechProfile : ApplicationProfile
{
    public override void Reset()
    {
        base.Reset();
        var solidFillLayerHandler = new SolidFillLayerHandler();
        solidFillLayerHandler.Properties._PrimaryColor = CommonColorUtils.FastColor(255, 255, 255, 24);
        Layers =
        [
            new Layer("Logitech Lightsync", new LogitechLayerHandler())
        ];
    }
}