using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Media;
using Aurora.Utils;
using Newtonsoft.Json;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using LinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;
using Point = System.Windows.Point;

namespace Aurora.EffectsEngine;

public class EffectBrush
{
    public enum BrushType
    {
        None,
        Solid,
        Linear,
        Radial
    }

    public enum BrushWrap
    {
        None,
        Repeat,
        Reflect
    }

    [JsonProperty("type")]
    public BrushType Type { get; } = BrushType.None;

    [JsonProperty("wrap")]
    public BrushWrap Wrap { get; } = BrushWrap.None;

    [JsonProperty("color_gradients")]
    [JsonConverter(typeof(SortedDictionaryAdapter))]
    public IReadOnlyDictionary<double, Color> ColorGradients => _colorGradients;

    [JsonProperty("start")]
    public PointF Start { get; set; }

    [JsonProperty("end")]
    public PointF End { get; set; }

    [JsonProperty("center")]
    public PointF Center { get; set; }

    private Brush? _drawingBrush;
    private System.Windows.Media.Brush? _mediaBrush;
    private SortedDictionary<double, Color> _colorGradients = new();

    [JsonConstructor]
    public EffectBrush(BrushType type, BrushWrap wrap, SortedDictionary<double, Color> colorGradients,
        PointF start, PointF end, PointF center, Brush drawingBrush, System.Windows.Media.Brush mediaBrush)
    {
        Type = type;
        Wrap = wrap;
        if (colorGradients.Any(kv => kv.Key > 1))
        {
            colorGradients.Clear();
        }
        if (!colorGradients.ContainsKey(0.0))
        {
            colorGradients[0] = Color.Transparent;
        }
        if (!colorGradients.ContainsKey(1.0))
        {
            colorGradients[1] = Color.Transparent;
        }
        _colorGradients = colorGradients;
        Start = start;
        End = end;
        Center = center;
        _drawingBrush = drawingBrush;
        _mediaBrush = mediaBrush;
    }

    public EffectBrush()
    {
        Type = BrushType.Solid;

        _colorGradients.Add(0.0f, Color.Red);
        _colorGradients.Add(1.0f, Color.Blue);

        Start = new PointF(0, 0);
        End = new PointF(1, 0);
        Center = new PointF(0.0f, 0.0f);
    }

    public EffectBrush(BrushType brushType, BrushWrap wrap = BrushWrap.None)
    {
        Type = brushType;
        Wrap = wrap;

        _colorGradients.Add(0.0f, Color.Red);
        _colorGradients.Add(1.0f, Color.Blue);

        Start = new PointF(0, 0);
        End = new PointF(1, 0);
        Center = new PointF(0.0f, 0.0f);
    }

    public EffectBrush(EffectBrush otherBrush)
    {
        Type = otherBrush.Type;
        Wrap = otherBrush.Wrap;
        _colorGradients = new SortedDictionary<double, Color>(otherBrush._colorGradients);
        Start = otherBrush.Start;
        End = otherBrush.End;
        Center = otherBrush.Center;
    }

    public EffectBrush(ColorSpectrum spectrum, BrushType brushType = BrushType.Linear, BrushWrap wrap = BrushWrap.None)
    {
        Type = brushType;
        Wrap = wrap;

        foreach(var color in spectrum.GetSpectrumColors())
            _colorGradients.Add(color.Key, color.Value);

        Start = new PointF(0, 0);
        End = new PointF(1, 0);
        Center = new PointF(0.0f, 0.0f);
    }

    public EffectBrush(Brush brush)
    {
        if (brush is SolidBrush solidBrush)
        {
            Type = BrushType.Solid;

            _colorGradients.Add(0.0f, solidBrush.Color);
            _colorGradients.Add(1.0f, solidBrush.Color);

            Wrap = BrushWrap.Repeat;
        }
        else if (brush is LinearGradientBrush lgb)
        {
            Type = BrushType.Linear;

            Start = lgb.Rectangle.Location;
            End = new PointF(lgb.Rectangle.Width, lgb.Rectangle.Height);
            Center = new PointF(0.0f, 0.0f);

            switch (lgb.WrapMode)
            {
                case WrapMode.Clamp:
                    Wrap = BrushWrap.None;
                    break;
                case WrapMode.Tile:
                    Wrap = BrushWrap.Repeat;
                    break;
                case WrapMode.TileFlipXY:
                    Wrap = BrushWrap.Reflect;
                    break;
            }

            try
            {
                if (lgb.InterpolationColors != null && lgb.InterpolationColors.Colors.Length == lgb.InterpolationColors.Positions.Length)
                {
                    for (var x = 0; x < lgb.InterpolationColors.Colors.Length; x++)
                    {
                        if (!ColorGradients.ContainsKey(lgb.InterpolationColors.Positions[x]) && lgb.InterpolationColors.Positions[x] >= 0.0f && lgb.InterpolationColors.Positions[x] <= 1.0f)
                            _colorGradients.Add(
                                lgb.InterpolationColors.Positions[x],
                                lgb.InterpolationColors.Colors[x]
                            );
                    }
                }
            }
            catch (Exception exc)
            {
                _colorGradients.Clear();

                for (var x = 0; x < lgb.LinearColors.Length; x++)
                {
                    var pos = x / (float)(lgb.LinearColors.Length - 1);

                    if (!ColorGradients.ContainsKey(pos))
                        _colorGradients.Add(
                            pos,
                            lgb.LinearColors[x]
                        );
                }
            }
        }
        else if (brush is PathGradientBrush pgb)
        {
            Type = BrushType.Radial;

            Start = pgb.Rectangle.Location;
            End = new PointF(pgb.Rectangle.Width, pgb.Rectangle.Height);
            Center = new PointF(
                pgb.CenterPoint.X,
                pgb.CenterPoint.Y
            );

            switch (pgb.WrapMode)
            {
                case WrapMode.Clamp:
                    Wrap = BrushWrap.None;
                    break;
                case WrapMode.Tile:
                    Wrap = BrushWrap.Repeat;
                    break;
                case WrapMode.TileFlipXY:
                    Wrap = BrushWrap.Reflect;
                    break;
            }

            try
            {
                if (pgb.InterpolationColors != null && pgb.InterpolationColors.Colors.Length == pgb.InterpolationColors.Positions.Length)
                {
                    for (var x = 0; x < pgb.InterpolationColors.Colors.Length; x++)
                    {
                        if (!ColorGradients.ContainsKey(pgb.InterpolationColors.Positions[x]) && pgb.InterpolationColors.Positions[x] >= 0.0f && pgb.InterpolationColors.Positions[x] <= 1.0f)
                            _colorGradients.Add(
                                pgb.InterpolationColors.Positions[x],
                                pgb.InterpolationColors.Colors[x]
                            );
                    }
                }
            }
            catch (Exception exc)
            {
                _colorGradients.Clear();

                for (var x = 0; x < pgb.SurroundColors.Length; x++)
                {
                    var pos = x / (float)(pgb.SurroundColors.Length - 1);

                    if (!ColorGradients.ContainsKey(pos))
                        _colorGradients.Add(
                            pos,
                            pgb.SurroundColors[x]
                        );
                }
            }
        }

        if(ColorGradients.Count > 0)
        {
            var firstFound = false;
            var firstColor = new Color();
            var lastColor = new Color();

            foreach(var kvp in ColorGradients)
            {
                if(!firstFound)
                {
                    firstColor = kvp.Value;
                    firstFound = true;
                }

                lastColor = kvp.Value;
            }

            if (!ColorGradients.ContainsKey(0.0f))
                _colorGradients.Add(0.0f, firstColor);

            if (!ColorGradients.ContainsKey(1.0f))
                _colorGradients.Add(1.0f, lastColor);
        }
        else
        {
            if (!ColorGradients.ContainsKey(0.0f))
                _colorGradients.Add(0.0f, Color.Transparent);

            if (!ColorGradients.ContainsKey(1.0f))
                _colorGradients.Add(1.0f, Color.Transparent);
        }

            
    }

    public EffectBrush(System.Windows.Media.Brush brush)
    {
        if (brush is SolidColorBrush colorBrush)
        {
            Type = BrushType.Solid;

            Wrap = BrushWrap.Repeat;

            _colorGradients.Add(0.0f, ColorUtils.MediaColorToDrawingColor(colorBrush.Color));
            _colorGradients.Add(1.0f, ColorUtils.MediaColorToDrawingColor(colorBrush.Color));
        }
        else if (brush is System.Windows.Media.LinearGradientBrush lgb)
        {
            Type = BrushType.Linear;

            Start = new PointF((float)lgb.StartPoint.X, (float)lgb.StartPoint.Y);
            End = new PointF((float)lgb.EndPoint.X, (float)lgb.EndPoint.Y);
            Center = new PointF(0.0f, 0.0f);

            switch (lgb.SpreadMethod)
            {
                case GradientSpreadMethod.Pad:
                    Wrap = BrushWrap.None;
                    break;
                case GradientSpreadMethod.Repeat:
                    Wrap = BrushWrap.Repeat;
                    break;
                case GradientSpreadMethod.Reflect:
                    Wrap = BrushWrap.Reflect;
                    break;
            }

            foreach (var grad in lgb.GradientStops)
            {
                if (!ColorGradients.ContainsKey((float)grad.Offset) && (float)grad.Offset >= 0.0f && (float)grad.Offset <= 1.0f)
                    _colorGradients.Add(
                        (float)grad.Offset,
                        ColorUtils.MediaColorToDrawingColor(grad.Color)
                    );
            }
        }
        else if (brush is RadialGradientBrush rgb)
        {
            Type = BrushType.Radial;

            Start = new PointF(0, 0);
            End = new PointF((float)rgb.RadiusX * 2.0f, (float)rgb.RadiusY * 2.0f);
            Center = new PointF(
                (float)rgb.Center.X,
                (float)rgb.Center.Y
            );

            switch (rgb.SpreadMethod)
            {
                case GradientSpreadMethod.Pad:
                    Wrap = BrushWrap.None;
                    break;
                case GradientSpreadMethod.Repeat:
                    Wrap = BrushWrap.Repeat;
                    break;
                case GradientSpreadMethod.Reflect:
                    Wrap = BrushWrap.Reflect;
                    break;
            }

            foreach (var grad in rgb.GradientStops)
            {
                if (!ColorGradients.ContainsKey((float)grad.Offset) && (float)grad.Offset >= 0.0f && (float)grad.Offset <= 1.0f)
                    _colorGradients.Add(
                        (float)grad.Offset,
                        ColorUtils.MediaColorToDrawingColor(grad.Color)
                    );
            }
        }

        if (ColorGradients.Count > 0)
        {
            var firstFound = false;
            var firstColor = new Color();
            var lastColor = new Color();

            foreach (var kvp in ColorGradients)
            {
                if (!firstFound)
                {
                    firstColor = kvp.Value;
                    firstFound = true;
                }

                lastColor = kvp.Value;
            }

            if (!ColorGradients.ContainsKey(0.0f))
                _colorGradients.Add(0.0f, firstColor);

            if (!ColorGradients.ContainsKey(1.0f))
                _colorGradients.Add(1.0f, lastColor);
        }
        else
        {
            if (!ColorGradients.ContainsKey(0.0f))
                _colorGradients.Add(0.0f, Color.Transparent);

            if (!ColorGradients.ContainsKey(1.0f))
                _colorGradients.Add(1.0f, Color.Transparent);
        }
    }

    public Brush GetDrawingBrush()
    {
        Brush returnBrush = Type switch
        {
            BrushType.Solid => new SolidBrush(ColorGradients[0.0f]),
            BrushType.Linear => GetLinearBrush(),
            BrushType.Radial => GetRadialBrush(),
            _ => new SolidBrush(Color.Transparent)
        };
            
        _drawingBrush = returnBrush;

        return _drawingBrush;
    }

    private LinearGradientBrush GetLinearBrush()
    {
        var brushColors = new List<Color>();
        var brushPositions = new List<float>();

        foreach (var kvp in ColorGradients)
        {
            brushPositions.Add((float)kvp.Key);
            brushColors.Add(kvp.Value);
        }

        var colorBlend = new ColorBlend
        {
            Colors = brushColors.ToArray(),
            Positions = brushPositions.ToArray()
        };
        var brush = new LinearGradientBrush(
            Start,
            End,
            Color.Red,
            Color.Red
        );
        brush.InterpolationColors = colorBlend;

        switch (Wrap)
        {
            case BrushWrap.Repeat:
                brush.WrapMode = WrapMode.Tile;
                break;
            case BrushWrap.Reflect:
                brush.WrapMode = WrapMode.TileFlipXY;
                break;
        }

        return brush;
    }

    private PathGradientBrush GetRadialBrush()
    {
        var gPath = new GraphicsPath();
        gPath.AddEllipse(
            new RectangleF(
                Start.X,
                Start.Y,
                End.X,
                End.Y
            ));

        var brush = new PathGradientBrush(
            gPath
        );

        switch (Wrap)
        {
            case BrushWrap.Repeat:
                brush.WrapMode = WrapMode.Tile;
                break;
            case BrushWrap.Reflect:
                brush.WrapMode = WrapMode.TileFlipXY;
                break;
        }

        var brushColors = new List<Color>();
        var brushPositions = new List<float>();

        foreach (var kvp in ColorGradients)
        {
            brushPositions.Add(1.0f - (float)kvp.Key);
            brushColors.Add(kvp.Value);
        }

        brush.CenterPoint = Center;

        brushColors.Reverse();
        brushPositions.Reverse();

        var colorBlend = new ColorBlend
        {
            Colors = brushColors.ToArray(),
            Positions = brushPositions.ToArray()
        };
        brush.InterpolationColors = colorBlend;

        return brush;
    }

    public System.Windows.Media.Brush GetMediaBrush()
    {
        if (_mediaBrush != null) return _mediaBrush;
        switch (Type)
        {
            case BrushType.Solid:
            {
                var brush = new SolidColorBrush(
                    ColorUtils.DrawingColorToMediaColor(ColorGradients[0.0f])
                );
                brush.Freeze();

                _mediaBrush = brush;
                break;
            }
            case BrushType.Linear:
            {
                var collection = new GradientStopCollection();

                foreach (var kvp in ColorGradients)
                {
                    collection.Add(
                        new GradientStop(
                            ColorUtils.DrawingColorToMediaColor(kvp.Value),
                            kvp.Key)
                    );
                }

                var brush = new System.Windows.Media.LinearGradientBrush(collection)
                {
                    StartPoint = new Point(Start.X, Start.Y),
                    EndPoint = new Point(End.X, End.Y)
                };

                brush.SpreadMethod = Wrap switch
                {
                    BrushWrap.None => GradientSpreadMethod.Pad,
                    BrushWrap.Repeat => GradientSpreadMethod.Repeat,
                    BrushWrap.Reflect => GradientSpreadMethod.Reflect,
                    _ => brush.SpreadMethod
                };

                _mediaBrush = brush;
                break;
            }
            case BrushType.Radial:
            {
                var collection = new GradientStopCollection();

                foreach (var kvp in ColorGradients)
                {
                    collection.Add(
                        new GradientStop(
                            ColorUtils.DrawingColorToMediaColor(kvp.Value),
                            kvp.Key)
                    );
                }

                var brush = new RadialGradientBrush(collection);
                brush.Center = new Point(Center.X, Center.Y);
                brush.RadiusX = End.X / 2.0;
                brush.RadiusY = End.Y / 2.0;

                brush.SpreadMethod = Wrap switch
                {
                    BrushWrap.None => GradientSpreadMethod.Pad,
                    BrushWrap.Repeat => GradientSpreadMethod.Repeat,
                    BrushWrap.Reflect => GradientSpreadMethod.Reflect,
                    _ => brush.SpreadMethod
                };

                _mediaBrush = brush;
                break;
            }
            default:
            {
                var brush = new SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(255, 255, 0, 0)
                );
                brush.Freeze();

                _mediaBrush = brush;
                break;
            }
        }

        return _mediaBrush;
    }

    public ColorSpectrum GetColorSpectrum()
    {
        var spectrum = new ColorSpectrum();

        if(Type == BrushType.Solid)
        {
            spectrum = new ColorSpectrum(ColorGradients[0.0f]);
        }
        else
        {
            foreach (var color in ColorGradients)
                spectrum.SetColorAt(color.Key, color.Value);
        }

        return spectrum;
    }

    /// <summary>
    /// Blends two EffectBrushes together by a specified amount
    /// </summary>
    /// <param name="otherBrush">The foreground EffectBrush (When percent is at 1.0D, only this EffectBrush is shown)</param>
    /// <param name="percent">The blending percent value</param>
    /// <returns>The blended EffectBrush</returns>
    public EffectBrush BlendEffectBrush(EffectBrush otherBrush, double percent)
    {
        if (percent <= 0.0)
            return new EffectBrush(this);
        if (percent >= 1.0)
            return new EffectBrush(otherBrush);

        var currentSpectrum = new ColorSpectrum(GetColorSpectrum());
        var newSpectrum = new ColorSpectrum(currentSpectrum).MultiplyByScalar(1.0 - percent);

        foreach (var (key, value) in otherBrush.ColorGradients)
        {
            var bgColor = currentSpectrum.GetColorAt(key);

            newSpectrum.SetColorAt(key, ColorUtils.BlendColors(bgColor, value, percent));
        }

        var returnBrush = new EffectBrush(newSpectrum, Type, Wrap)
        {
            Start = new PointF((float)(Start.X * (1.0 - percent) + otherBrush.Start.X * percent), (float)(Start.Y * (1.0 - percent) + otherBrush.Start.Y * percent)),
            End = new PointF((float)(End.X * (1.0 - percent) + otherBrush.End.X * percent), (float)(End.Y * (1.0 - percent) + otherBrush.End.Y * percent)),
            Center = new PointF((float)(Center.X * (1.0 - percent) + otherBrush.Center.X * percent), (float)(Center.Y * (1.0 - percent) + otherBrush.Center.Y * percent))
        };

        return returnBrush;
    }

    public EffectBrush SetColorGradients(SortedDictionary<double, Color> gradients)
    {
        _colorGradients = gradients;
        return this;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((EffectBrush)obj);
    }

    public bool Equals(EffectBrush p)
    {
        if (ReferenceEquals(null, p)) return false;
        if (ReferenceEquals(this, p)) return true;

        return Type == p.Type &&
               Wrap == p.Wrap &&
               ColorGradients.Equals(p.ColorGradients) &&
               Start.Equals(p.Start) &&
               End.Equals(p.End) &&
               Center.Equals(p.Center);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + Wrap.GetHashCode();
            hash = hash * 23 + ColorGradients.GetHashCode();
            hash = hash * 23 + Start.GetHashCode();
            hash = hash * 23 + End.GetHashCode();
            hash = hash * 23 + Center.GetHashCode();
            return hash;
        }
    }
}