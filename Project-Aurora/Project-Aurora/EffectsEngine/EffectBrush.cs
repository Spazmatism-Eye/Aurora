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

namespace Aurora.EffectsEngine
{
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

        public BrushType type = BrushType.None;
        public BrushWrap wrap = BrushWrap.None;
        [JsonProperty("color_gradients")]
        [JsonConverter(typeof(SortedDictionaryAdapter))]
        public SortedDictionary<double, Color> colorGradients = new();
        public PointF start;
        public PointF end;
        public PointF center;

        private Brush _drawingBrush;
        private System.Windows.Media.Brush _mediaBrush;

        [JsonConstructor]
        public EffectBrush(BrushType type, BrushWrap wrap, SortedDictionary<double, Color> colorGradients,
            PointF start, PointF end, PointF center, Brush drawingBrush, System.Windows.Media.Brush mediaBrush)
        {
            this.type = type;
            this.wrap = wrap;
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
            this.colorGradients = colorGradients;
            this.start = start;
            this.end = end;
            this.center = center;
            _drawingBrush = drawingBrush;
            _mediaBrush = mediaBrush;
        }

        public EffectBrush()
        {
            type = BrushType.Solid;

            colorGradients.Add(0.0f, Color.Red);
            colorGradients.Add(1.0f, Color.Blue);

            start = new PointF(0, 0);
            end = new PointF(1, 0);
            center = new PointF(0.0f, 0.0f);
        }

        public EffectBrush(EffectBrush otherBrush)
        {
            type = otherBrush.type;
            wrap = otherBrush.wrap;
            colorGradients = otherBrush.colorGradients;
            start = otherBrush.start;
            end = otherBrush.end;
            center = otherBrush.center;
        }

        public EffectBrush(ColorSpectrum spectrum)
        {
            type = BrushType.Linear;

            foreach(var color in spectrum.GetSpectrumColors())
                colorGradients.Add(color.Key, color.Value);

            start = new PointF(0, 0);
            end = new PointF(1, 0);
            center = new PointF(0.0f, 0.0f);
        }

        public EffectBrush(Brush brush)
        {
            colorGradients = new SortedDictionary<double, Color>();
            if (brush is SolidBrush)
            {
                type = BrushType.Solid;

                colorGradients.Add(0.0f, (brush as SolidBrush).Color);
                colorGradients.Add(1.0f, (brush as SolidBrush).Color);

                wrap = BrushWrap.Repeat;
            }
            else if (brush is LinearGradientBrush)
            {
                type = BrushType.Linear;

                LinearGradientBrush lgb = (brush as LinearGradientBrush);

                start = lgb.Rectangle.Location;
                end = new PointF(lgb.Rectangle.Width, lgb.Rectangle.Height);
                center = new PointF(0.0f, 0.0f);

                switch (lgb.WrapMode)
                {
                    case (WrapMode.Clamp):
                        wrap = BrushWrap.None;
                        break;
                    case (WrapMode.Tile):
                        wrap = BrushWrap.Repeat;
                        break;
                    case (WrapMode.TileFlipXY):
                        wrap = BrushWrap.Reflect;
                        break;
                }

                try
                {
                    if (lgb.InterpolationColors != null && lgb.InterpolationColors.Colors.Length == lgb.InterpolationColors.Positions.Length)
                    {
                        for (int x = 0; x < lgb.InterpolationColors.Colors.Length; x++)
                        {
                            if (!colorGradients.ContainsKey(lgb.InterpolationColors.Positions[x]) && (lgb.InterpolationColors.Positions[x] >= 0.0f && lgb.InterpolationColors.Positions[x] <= 1.0f))
                                colorGradients.Add(
                                    lgb.InterpolationColors.Positions[x],
                                    lgb.InterpolationColors.Colors[x]
                                    );
                        }
                    }
                }
                catch (Exception exc)
                {
                    colorGradients.Clear();

                    for (int x = 0; x < lgb.LinearColors.Length; x++)
                    {
                        float pos = x / (float)(lgb.LinearColors.Length - 1);

                        if (!colorGradients.ContainsKey(pos))
                            colorGradients.Add(
                                pos,
                                lgb.LinearColors[x]
                                );
                    }
                }
            }
            else if (brush is PathGradientBrush)
            {
                type = BrushType.Radial;

                PathGradientBrush pgb = (brush as PathGradientBrush);

                start = pgb.Rectangle.Location;
                end = new PointF(pgb.Rectangle.Width, pgb.Rectangle.Height);
                center = new PointF(
                    pgb.CenterPoint.X,
                    pgb.CenterPoint.Y
                    );


                switch (pgb.WrapMode)
                {
                    case (WrapMode.Clamp):
                        wrap = BrushWrap.None;
                        break;
                    case (WrapMode.Tile):
                        wrap = BrushWrap.Repeat;
                        break;
                    case (WrapMode.TileFlipXY):
                        wrap = BrushWrap.Reflect;
                        break;
                }

                try
                {
                    if (pgb.InterpolationColors != null && pgb.InterpolationColors.Colors.Length == pgb.InterpolationColors.Positions.Length)
                    {
                        for (int x = 0; x < pgb.InterpolationColors.Colors.Length; x++)
                        {
                            if (!colorGradients.ContainsKey(pgb.InterpolationColors.Positions[x]) && (pgb.InterpolationColors.Positions[x] >= 0.0f && pgb.InterpolationColors.Positions[x] <= 1.0f))
                                colorGradients.Add(
                                    pgb.InterpolationColors.Positions[x],
                                    pgb.InterpolationColors.Colors[x]
                                    );
                        }
                    }
                }
                catch (Exception exc)
                {
                    colorGradients.Clear();

                    for (int x = 0; x < pgb.SurroundColors.Length; x++)
                    {
                        float pos = x / (float)(pgb.SurroundColors.Length - 1);

                        if (!colorGradients.ContainsKey(pos))
                            colorGradients.Add(
                                pos,
                                pgb.SurroundColors[x]
                                );
                    }
                }
            }

            if(colorGradients.Count > 0)
            {
                bool firstFound = false;
                Color first_color = new Color();
                Color last_color = new Color();

                foreach(var kvp in colorGradients)
                {
                    if(!firstFound)
                    {
                        first_color = kvp.Value;
                        firstFound = true;
                    }

                    last_color = kvp.Value;
                }

                if (!colorGradients.ContainsKey(0.0f))
                    colorGradients.Add(0.0f, first_color);

                if (!colorGradients.ContainsKey(1.0f))
                    colorGradients.Add(1.0f, last_color);
            }
            else
            {
                if (!colorGradients.ContainsKey(0.0f))
                    colorGradients.Add(0.0f, Color.Transparent);

                if (!colorGradients.ContainsKey(1.0f))
                    colorGradients.Add(1.0f, Color.Transparent);
            }

            
        }

        public EffectBrush(System.Windows.Media.Brush brush)
        {
            if (brush is SolidColorBrush)
            {
                type = BrushType.Solid;

                wrap = BrushWrap.Repeat;

                colorGradients.Add(0.0f, ColorUtils.MediaColorToDrawingColor((brush as SolidColorBrush).Color));
                colorGradients.Add(1.0f, ColorUtils.MediaColorToDrawingColor((brush as SolidColorBrush).Color));
            }
            else if (brush is System.Windows.Media.LinearGradientBrush)
            {
                type = BrushType.Linear;

                System.Windows.Media.LinearGradientBrush lgb = (brush as System.Windows.Media.LinearGradientBrush);

                start = new PointF((float)lgb.StartPoint.X, (float)lgb.StartPoint.Y);
                end = new PointF((float)lgb.EndPoint.X, (float)lgb.EndPoint.Y);
                center = new PointF(0.0f, 0.0f);

                switch (lgb.SpreadMethod)
                {
                    case (GradientSpreadMethod.Pad):
                        wrap = BrushWrap.None;
                        break;
                    case (GradientSpreadMethod.Repeat):
                        wrap = BrushWrap.Repeat;
                        break;
                    case (GradientSpreadMethod.Reflect):
                        wrap = BrushWrap.Reflect;
                        break;
                }

                foreach (var grad in lgb.GradientStops)
                {
                    if (!colorGradients.ContainsKey((float)grad.Offset) && ((float)grad.Offset >= 0.0f && (float)grad.Offset <= 1.0f))
                        colorGradients.Add(
                            (float)grad.Offset,
                            ColorUtils.MediaColorToDrawingColor(grad.Color)
                            );
                }
            }
            else if (brush is RadialGradientBrush)
            {
                type = BrushType.Radial;

                RadialGradientBrush rgb = (brush as RadialGradientBrush);

                start = new PointF(0, 0);
                end = new PointF((float)rgb.RadiusX * 2.0f, (float)rgb.RadiusY * 2.0f);
                center = new PointF(
                    (float)rgb.Center.X,
                    (float)rgb.Center.Y
                    );

                switch (rgb.SpreadMethod)
                {
                    case (GradientSpreadMethod.Pad):
                        wrap = BrushWrap.None;
                        break;
                    case (GradientSpreadMethod.Repeat):
                        wrap = BrushWrap.Repeat;
                        break;
                    case (GradientSpreadMethod.Reflect):
                        wrap = BrushWrap.Reflect;
                        break;
                }

                foreach (var grad in rgb.GradientStops)
                {
                    if (!colorGradients.ContainsKey((float)grad.Offset) && ((float)grad.Offset >= 0.0f && (float)grad.Offset <= 1.0f))
                        colorGradients.Add(
                            (float)grad.Offset,
                            ColorUtils.MediaColorToDrawingColor(grad.Color)
                            );
                }
            }

            if (colorGradients.Count > 0)
            {
                bool firstFound = false;
                Color first_color = new Color();
                Color last_color = new Color();

                foreach (var kvp in colorGradients)
                {
                    if (!firstFound)
                    {
                        first_color = kvp.Value;
                        firstFound = true;
                    }

                    last_color = kvp.Value;
                }

                if (!colorGradients.ContainsKey(0.0f))
                    colorGradients.Add(0.0f, first_color);

                if (!colorGradients.ContainsKey(1.0f))
                    colorGradients.Add(1.0f, last_color);
            }
            else
            {
                if (!colorGradients.ContainsKey(0.0f))
                    colorGradients.Add(0.0f, Color.Transparent);

                if (!colorGradients.ContainsKey(1.0f))
                    colorGradients.Add(1.0f, Color.Transparent);
            }
        }

        public EffectBrush SetBrushType(BrushType type)
        {
            this.type = type;
            return this;
        }

        public EffectBrush SetWrap(BrushWrap wrap)
        {
            this.wrap = wrap;
            return this;
        }

        public Brush GetDrawingBrush()
        {
            Brush returnBrush = type switch
            {
                BrushType.Solid => new SolidBrush(colorGradients[0.0f]),
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

            foreach (var kvp in colorGradients)
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
                start,
                end,
                Color.Red,
                Color.Red
            );
            brush.InterpolationColors = colorBlend;

            switch (wrap)
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
                    start.X,
                    start.Y,
                    end.X,
                    end.Y
                ));

            var brush = new PathGradientBrush(
                gPath
            );

            switch (wrap)
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

            foreach (var kvp in colorGradients)
            {
                brushPositions.Add(1.0f - (float)kvp.Key);
                brushColors.Add(kvp.Value);
            }

            brush.CenterPoint = center;

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
            switch (type)
            {
                case BrushType.Solid:
                {
                    var brush = new SolidColorBrush(
                        ColorUtils.DrawingColorToMediaColor(colorGradients[0.0f])
                    );
                    brush.Freeze();

                    _mediaBrush = brush;
                    break;
                }
                case BrushType.Linear:
                {
                    var collection = new GradientStopCollection();

                    foreach (var kvp in colorGradients)
                    {
                        collection.Add(
                            new GradientStop(
                                ColorUtils.DrawingColorToMediaColor(kvp.Value),
                                kvp.Key)
                        );
                    }

                    var brush = new System.Windows.Media.LinearGradientBrush(collection)
                    {
                        StartPoint = new Point(start.X, start.Y),
                        EndPoint = new Point(end.X, end.Y)
                    };

                    brush.SpreadMethod = wrap switch
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

                    foreach (var kvp in colorGradients)
                    {
                        collection.Add(
                            new GradientStop(
                                ColorUtils.DrawingColorToMediaColor(kvp.Value),
                                kvp.Key)
                        );
                    }

                    var brush = new RadialGradientBrush(collection);
                    brush.Center = new Point(center.X, center.Y);
                    brush.RadiusX = end.X / 2.0;
                    brush.RadiusY = end.Y / 2.0;

                    brush.SpreadMethod = wrap switch
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
            ColorSpectrum spectrum = new ColorSpectrum();

            if(type == BrushType.Solid)
            {
                spectrum = new ColorSpectrum(colorGradients[0.0f]);
            }
            else
            {
                foreach (var color in colorGradients)
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

            ColorSpectrum currentSpectrum = new ColorSpectrum(GetColorSpectrum());
            ColorSpectrum newSpectrum = new ColorSpectrum(currentSpectrum).MultiplyByScalar(1.0 - percent);

            foreach (var kvp in otherBrush.colorGradients)
            {
                Color bgColor = currentSpectrum.GetColorAt(kvp.Key);
                Color fgColor = kvp.Value;

                newSpectrum.SetColorAt(kvp.Key, ColorUtils.BlendColors(bgColor, fgColor, percent));
            }

            EffectBrush returnBrush = new EffectBrush(newSpectrum);
            returnBrush.SetBrushType(type).SetWrap(wrap);

            returnBrush.start = new PointF((float)(start.X * (1.0 - percent) + otherBrush.start.X * (percent)), (float)(start.Y * (1.0 - percent) + otherBrush.start.Y * (percent)));
            returnBrush.end = new PointF((float)(end.X * (1.0 - percent) + otherBrush.end.X * (percent)), (float)(end.Y * (1.0 - percent) + otherBrush.end.Y * (percent)));
            returnBrush.center = new PointF((float)(center.X * (1.0 - percent) + otherBrush.center.X * (percent)), (float)(center.Y * (1.0 - percent) + otherBrush.center.Y * (percent)));

            return returnBrush;
        }

        public override bool Equals(object obj)
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

            return (type == p.type &&
                wrap == p.wrap &&
                colorGradients.Equals(p.colorGradients) &&
                start.Equals(p.start) &&
                end.Equals(p.end) &&
                center.Equals(p.center));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + type.GetHashCode();
                hash = hash * 23 + wrap.GetHashCode();
                hash = hash * 23 + colorGradients.GetHashCode();
                hash = hash * 23 + start.GetHashCode();
                hash = hash * 23 + end.GetHashCode();
                hash = hash * 23 + center.GetHashCode();
                return hash;
            }
        }
    }
}
