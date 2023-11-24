using System.Drawing;
using System.Text.Json.Serialization;
using Common.Utils;

namespace Common;

public readonly record struct SimpleColor
{
    public static readonly SimpleColor White = new(255, 255, 255);
    public static readonly SimpleColor Black = new(0, 0, 0);

    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
    public byte A { get; }

    [JsonConstructor]
    public SimpleColor(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public int ToArgb() => (A << 24) | (R << 16) | (G << 8) | B;

    public static explicit operator Color(SimpleColor color)
    {
        return CommonColorUtils.FastColor(color.R, color.G, color.B, color.A);
    }

    public static explicit operator SimpleColor(Color color)
    {
        return new SimpleColor(color.R, color.G, color.B, color.A);
    }

    public static SimpleColor FromArgb(int argb)
    {
        const int argbAlphaShift = 24;
        const int argbRedShift = 16;
        const int argbGreenShift = 8;
        const int argbBlueShift = 0;
        
        var r = unchecked((byte)(argb >> argbRedShift));
        var g = unchecked((byte)(argb >> argbGreenShift));
        var b = unchecked((byte)(argb >> argbBlueShift));
        var a = unchecked((byte)(argb >> argbAlphaShift));

        return new SimpleColor(r, g, b, a);
    }
}