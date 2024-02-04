using System.Collections.Immutable;
using RGB.NET.Core;

namespace Common.Devices.RGBNet;

public static class LedGroups
{
    public static readonly ImmutableHashSet<LedId> KeyboardLeds = Enum.GetValues(typeof(LedId)).Cast<LedId>()
        .Where(id => id.ToString().StartsWith("Keyboard_"))
        .ToImmutableHashSet();

    public static readonly ImmutableHashSet<LedId> MouseLeds = Enum.GetValues(typeof(LedId)).Cast<LedId>()
        .Where(id => id.ToString().StartsWith("Mouse"))
        .ToImmutableHashSet();

    public static readonly ImmutableHashSet<LedId> MousepadLeds = Enum.GetValues(typeof(LedId)).Cast<LedId>()
        .Where(id => id.ToString().StartsWith("Mousepad"))
        .ToImmutableHashSet();

    public static readonly ImmutableHashSet<LedId> HeadsetLeds = Enum.GetValues(typeof(LedId)).Cast<LedId>()
        .Where(id => id.ToString().StartsWith("Headset"))
        .ToImmutableHashSet();
}