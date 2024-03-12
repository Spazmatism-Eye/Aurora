using System.Runtime.InteropServices;

namespace AuroraRgb.Utils;

public static partial class Kernel32
{
    [LibraryImport("kernel32.dll", EntryPoint = "WTSGetActiveConsoleSessionId")]
    internal static partial uint WtsGetActiveConsoleSessionId();
}