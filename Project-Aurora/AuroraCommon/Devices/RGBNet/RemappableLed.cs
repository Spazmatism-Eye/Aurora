using RGB.NET.Core;

namespace Common.Devices.RGBNet;

public record struct RemappableLed(LedId LedId, DeviceKeys RemappedKey, SimpleColor ColorOverride)
{
    public LedId LedId = LedId;
    public DeviceKeys RemappedKey = RemappedKey;
    public SimpleColor ColorOverride = ColorOverride;
}