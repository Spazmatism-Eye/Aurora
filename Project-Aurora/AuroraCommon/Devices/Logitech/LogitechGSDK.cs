using System.Runtime.InteropServices;
using Common.Utils;

namespace Common.Devices.Logitech;

public enum KeyboardNames
{
    ESC = 0x01,
    F1 = 0x3b,
    F2 = 0x3c,
    F3 = 0x3d,
    F4 = 0x3e,
    F5 = 0x3f,
    F6 = 0x40,
    F7 = 0x41,
    F8 = 0x42,
    F9 = 0x43,
    F10 = 0x44,
    F11 = 0x57,
    F12 = 0x58,
    PRINT_SCREEN = 0x137,
    SCROLL_LOCK = 0x46,
    PAUSE_BREAK = 0x145,
    TILDE = 0x29,
    ONE = 0x02,
    TWO = 0x03,
    THREE = 0x04,
    FOUR = 0x05,
    FIVE = 0x06,
    SIX = 0x07,
    SEVEN = 0x08,
    EIGHT = 0x09,
    NINE = 0x0A,
    ZERO = 0x0B,
    MINUS = 0x0C,
    EQUALS = 0x0D,
    BACKSPACE = 0x0E,
    INSERT = 0x152,
    HOME = 0x147,
    PAGE_UP = 0x149,
    NUM_LOCK = 0x45,
    NUM_SLASH = 0x135,
    NUM_ASTERISK = 0x37,
    NUM_MINUS = 0x4A,
    TAB = 0x0F,
    Q = 0x10,
    W = 0x11,
    E = 0x12,
    R = 0x13,
    T = 0x14,
    Y = 0x15,
    U = 0x16,
    I = 0x17,
    O = 0x18,
    P = 0x19,
    OPEN_BRACKET = 0x1A,
    CLOSE_BRACKET = 0x1B,
    BACKSLASH = 0x2B,
    KEYBOARD_DELETE = 0x153,
    END = 0x14F,
    PAGE_DOWN = 0x151,
    NUM_SEVEN = 0x47,
    NUM_EIGHT = 0x48,
    NUM_NINE = 0x49,
    NUM_PLUS = 0x4E,
    CAPS_LOCK = 0x3A,
    A = 0x1E,
    S = 0x1F,
    D = 0x20,
    F = 0x21,
    G = 0x22,
    H = 0x23,
    J = 0x24,
    K = 0x25,
    L = 0x26,
    SEMICOLON = 0x27,
    APOSTROPHE = 0x28,
    ENTER = 0x1C,
    NUM_FOUR = 0x4B,
    NUM_FIVE = 0x4C,
    NUM_SIX = 0x4D,
    LEFT_SHIFT = 0x2A,
    Z = 0x2C,
    X = 0x2D,
    C = 0x2E,
    V = 0x2F,
    B = 0x30,
    N = 0x31,
    M = 0x32,
    COMMA = 0x33,
    PERIOD = 0x34,
    FORWARD_SLASH = 0x35,
    RIGHT_SHIFT = 0x36,
    ARROW_UP = 0x148,
    NUM_ONE = 0x4F,
    NUM_TWO = 0x50,
    NUM_THREE = 0x51,
    NUM_ENTER = 0x11C,
    LEFT_CONTROL = 0x1D,
    LEFT_WINDOWS = 0x15B,
    LEFT_ALT = 0x38,
    SPACE = 0x39,
    RIGHT_ALT = 0x138,
    RIGHT_WINDOWS = 0x15C,
    APPLICATION_SELECT = 0x15D,
    RIGHT_CONTROL = 0x11D,
    ARROW_LEFT = 0x14B,
    ARROW_DOWN = 0x150,
    ARROW_RIGHT = 0x14D,
    NUM_ZERO = 0x52,
    NUM_PERIOD = 0x53,
    G_1 = 0xFFF1,
    G_2 = 0xFFF2,
    G_3 = 0xFFF3,
    G_4 = 0xFFF4,
    G_5 = 0xFFF5,
    G_6 = 0xFFF6,
    G_7 = 0xFFF7,
    G_8 = 0xFFF8,
    G_9 = 0xFFF9,
    G_LOGO = 0xFFFF1,
    G_BADGE = 0xFFFF2
}

public enum DeviceType
{
    Keyboard = 0x0,
    Mouse = 0x3,
    Mousemat = 0x4,
    Headset = 0x8,
    Speaker = 0xe
}

public enum LGDLL
{
    LGS,
    GHUB
}

public static class LogitechGSDK
{
    public const int LOGI_LED_BITMAP_WIDTH = 21;
    public const int LOGI_LED_BITMAP_HEIGHT = 6;
    public const int LOGI_LED_BITMAP_BYTES_PER_KEY = 4;

    public const int LOGI_LED_BITMAP_SIZE = LOGI_LED_BITMAP_WIDTH * LOGI_LED_BITMAP_HEIGHT * LOGI_LED_BITMAP_BYTES_PER_KEY;

    public static bool GHUB = true;

    public static bool LogiLedInit()
    {
        return GHUB ? GHUBImports.LogiLedInit() : LGSImports.LogiLedInit();
    }

    public static bool LogiLedSetLighting(SimpleColor color)
    {
        var (R, G, B) = GetColorValues(color);
        return GHUB ? GHUBImports.LogiLedSetLighting(R, G, B) : LGSImports.LogiLedSetLighting(R, G, B);
    }

    public static bool LogiLedSetLightingFromBitmap(byte[] bitmap)
    {
        return GHUB ? GHUBImports.LogiLedSetLightingFromBitmap(bitmap) :
            LGSImports.LogiLedSetLightingFromBitmap(bitmap);
    }

    public static bool LogiLedSetLightingForKeyWithKeyName(KeyboardNames keyCode, SimpleColor color)
    {
        var (R, G, B) = GetColorValues(color);
        return GHUB ? GHUBImports.LogiLedSetLightingForKeyWithKeyName(keyCode, R, G, B) :
            LGSImports.LogiLedSetLightingForKeyWithKeyName(keyCode, R, G, B);
    }

    public static bool LogiLedSetLightingForTargetZone(DeviceType deviceType, int zone, SimpleColor color)
    {
        var (R, G, B) = GetColorValues(color);
        return GHUB ? GHUBImports.LogiLedSetLightingForTargetZone((byte)deviceType, zone, R, G, B) :
            LGSImports.LogiLedSetLightingForTargetZone((byte)deviceType, zone, R, G, B);
    }

    public static void LogiLedShutdown()
    {
        if (GHUB)
            GHUBImports.LogiLedShutdown();
        else
            LGSImports.LogiLedShutdown();
    }

    public static bool LogiLedSaveCurrentLighting()
    {
        return GHUB ? GHUBImports.LogiLedSaveCurrentLighting() : LGSImports.LogiLedSaveCurrentLighting();
    }

    public static bool LogiLedRestoreLighting()
    {
        return GHUB ? GHUBImports.LogiLedRestoreLighting() : LGSImports.LogiLedRestoreLighting();
    }

    private static (int R, int G, int B) GetColorValues(SimpleColor clr)
    {
        clr = CommonColorUtils.CorrectWithAlpha(clr);
        return ((int)(clr.R / 255.0 * 100.0),
            (int)(clr.G / 255.0 * 100.0),
            (int)(clr.B / 255.0 * 100.0));
    }
}

internal static class LGSImports
{
    private const string dllpath = "Logi\\LGS\\LogitechLed.dll";

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedInit();

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSaveCurrentLighting();

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedRestoreLighting();

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLightingFromBitmap(byte[] bitmap);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLightingForKeyWithKeyName(KeyboardNames keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLightingForTargetZone(byte deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LogiLedShutdown();
}

internal static class GHUBImports
{
    private const string dllpath = "Logi\\GHUB\\LogitechLed.dll";

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedInit();

    //Config option functions

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSaveCurrentLighting();

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedRestoreLighting();

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLightingFromBitmap(byte[] bitmap);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLightingForKeyWithKeyName(KeyboardNames keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool LogiLedSetLightingForTargetZone(byte deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport(dllpath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LogiLedShutdown();
}