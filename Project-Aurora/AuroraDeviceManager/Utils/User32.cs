using System.Runtime.InteropServices;

namespace AuroraDeviceManager.Utils;

internal static partial class Kernel32
{
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial void SetProcessShutdownParameters(uint dwLevel, uint dwFlags);
}