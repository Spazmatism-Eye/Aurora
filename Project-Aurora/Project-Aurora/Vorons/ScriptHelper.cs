using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using AuroraRgb.EffectsEngine;
using Common.Devices;

namespace AuroraRgb.Vorons;

public static class ScriptHelper
{
        
    public static void RegProp(this VariableRegistry registry,
        string name, object defaultValue, string remark = "", object? min = null, object? max = null)
    {
        registry.Register(name, defaultValue, name, max, min, remark);
    }

    public static ColorSpectrum StringToSpectrum(string text)
    {
        var colors = text.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().Split('@').Select(x2 => x2.Trim()).ToArray())
            .ToArray();
        var spectrum = new ColorSpectrum();
        foreach (var colorSet in colors.Select((x, i) => new
                 {
                     Color = ColorTranslator.FromHtml(x[0]),
                     Position = x.Length > 1 ? float.Parse(x[1], CultureInfo.InvariantCulture) : 1f / (colors.Length - 1) * i
                 }))
        {
            spectrum.SetColorAt(colorSet.Position, colorSet.Color);
        }
        return spectrum;
    }
}