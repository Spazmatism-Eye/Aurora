using System.Runtime.InteropServices;
using AuroraRgb.Modules.Logitech.Enums;

namespace AuroraRgb.Modules.Logitech.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct LogitechSetTargetZone
{
    public LogiDeviceType DeviceType;
    public int ZoneId;
    public LogitechRgbColor RgbColor;
}