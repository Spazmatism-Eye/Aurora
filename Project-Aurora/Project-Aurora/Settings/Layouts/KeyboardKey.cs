using System.Text.Json.Serialization;
using Common.Devices;

namespace Aurora.Settings.Layouts;

public class KeyboardKey {
    private DeviceKeys? _tag;
    private double? _marginLeft;
    private double? _marginTop;
    private double? _width;
    private double? _height;
    private double? _fontSize;
    private bool? _enabled;
    private bool? _lineBreak;
    private bool? _absoluteLocation;

    [JsonPropertyName("visualName")]
    public string? VisualName { get; set; }

    [JsonPropertyName("tag")]
    public DeviceKeys Tag
    {
        get => _tag.GetValueOrDefault();
        set => _tag = value;
    }

    [JsonPropertyName("line_break")]
    public bool LineBreak
    {
        get => _lineBreak.GetValueOrDefault();
        set => _lineBreak = value;
    }

    [JsonPropertyName("margin_left")]
    public double MarginLeft
    {
        get => _marginLeft.GetValueOrDefault();
        set => _marginLeft = value;
    }

    [JsonPropertyName("margin_top")]
    public double MarginTop
    {
        get => _marginTop.GetValueOrDefault();
        set => _marginTop = value;
    }

    [JsonPropertyName("width")]
    public double Width
    {
        get => _width.GetValueOrDefault(30);
        set => _width = value;
    }

    [JsonPropertyName("height")]
    public double Height
    {
        get => _height.GetValueOrDefault(30);
        set => _height = value;
    }

    [JsonPropertyName("font_size")]
    public double FontSize
    {
        get => _fontSize.GetValueOrDefault(12);
        set => _fontSize = value;
    }

    [JsonPropertyName("enabled")]
    public bool Enabled
    {
        get => _enabled.GetValueOrDefault(true);
        set => _enabled = value;
    }

    [JsonPropertyName("absolute_location")]
    public bool AbsoluteLocation
    {
        get => _absoluteLocation.GetValueOrDefault();
        set => _absoluteLocation = value;
    }

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("z_index")]
    public int ZIndex { get; set; }

    public void UpdateFromOtherKey(KeyboardKey otherKey)
    {
        if (otherKey.VisualName != null) VisualName = otherKey.VisualName;
        if (otherKey._tag != null) Tag = otherKey.Tag;
        if (otherKey._lineBreak != null) LineBreak = otherKey.LineBreak;
        if (otherKey._width != null) Width = otherKey.Width;
        if (otherKey._height != null) Height = otherKey.Height;
        if (otherKey._fontSize != null) FontSize = otherKey.FontSize;
        if (otherKey._marginLeft != null) MarginLeft = otherKey.MarginLeft;
        if (otherKey._marginTop != null) MarginTop = otherKey.MarginTop;
        if (otherKey._enabled != null) Enabled = otherKey.Enabled;
        if (otherKey._absoluteLocation != null) AbsoluteLocation = otherKey.AbsoluteLocation;
    }
}