using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using Common.Devices;

namespace AuroraRgb.Settings.Layouts;

public class VirtualGroup
{
    [JsonPropertyName("origin_region")]
    public KeyboardRegion OriginRegion { get; set; }

    [JsonPropertyName("grouped_keys")]
    public List<KeyboardKey> GroupedKeys { get; set; } = [];

    private RectangleF _region = new(0, 0, 0, 0);

    [JsonPropertyName("region")]
    public RectangleF Region => _region;

    [JsonPropertyName("key_conversion")]
    public Dictionary<DeviceKeys, DeviceKeys> KeyConversion { get; set; } = new();

    public void AddFeature(KeyboardKey[] keys, KeyboardRegion insertionRegion = KeyboardRegion.TopLeft)
    {
        var locationX = 0.0D;
        var locationY = 0.0D;

        switch (insertionRegion)
        {
            case KeyboardRegion.TopRight:
                locationX = _region.Width;
                break;
            case KeyboardRegion.BottomLeft:
                locationY = _region.Height;
                break;
            case KeyboardRegion.BottomRight:
                locationX = _region.Width;
                locationY = _region.Height;
                break;
        }

        var addedWidth = 0.0f;
        var addedHeight = 0.0f;

        foreach (var key in keys)
        {
            key.MarginLeft += locationX;
            key.MarginTop += locationY;

            GroupedKeys.Add(key);

            if (key.Width + key.MarginLeft > _region.Width)
                _region.Width = (float) (key.Width + key.MarginLeft);
            else if (key.MarginLeft + addedWidth < 0)
            {
                addedWidth = -(float) key.MarginLeft;
                _region.Width -= (float) key.MarginLeft;
            }

            if (key.Height + key.MarginTop > _region.Height)
                _region.Height = (float) (key.Height + key.MarginTop);
            else if (key.MarginTop + addedHeight < 0)
            {
                addedHeight = -(float) key.MarginTop;
                _region.Height -= (float) key.MarginTop;
            }
        }

        NormalizeKeys();
    }

    private void NormalizeKeys()
    {
        var xCorrection = 0.0D;
        var yCorrection = 0.0D;

        foreach (var key in GroupedKeys.Where(key => key.AbsoluteLocation))
        {
            if (key.MarginLeft < xCorrection)
                xCorrection = key.MarginLeft;

            if (key.MarginTop < yCorrection)
                yCorrection = key.MarginTop;
        }

        if (GroupedKeys.Count <= 0) return;
        GroupedKeys[0].MarginTop -= yCorrection;

        var previousLinebreak = true;
        foreach (var key in GroupedKeys)
        {
            if (key.AbsoluteLocation)
            {
                key.MarginTop -= yCorrection;
                key.MarginLeft -= xCorrection;
            }
            else
            {
                if (previousLinebreak && !key.LineBreak)
                {
                    key.MarginLeft -= xCorrection;
                }

                previousLinebreak = key.LineBreak;
            }
        }
    }

    internal void AdjustKeys(Dictionary<DeviceKeys, KeyboardKey> keys)
    {
        var applicableKeys = GroupedKeys.FindAll(key => keys.ContainsKey(key.Tag));

        foreach (var key in applicableKeys)
        {
            var otherKey = keys[key.Tag];
            key.UpdateFromOtherKey(otherKey);
        }
    }

    internal void RemoveKeys(DeviceKeys[] keysToRemove)
    {
        GroupedKeys.RemoveAll(key => keysToRemove.Contains(key.Tag));

        double layoutHeight = 0;
        double layoutWidth = 0;
        double currentHeight = 0;
        double currentWidth = 0;

        foreach (var key in GroupedKeys)
        {
            if (key.Width + key.MarginLeft > 0)
                currentWidth += key.Width + key.MarginLeft;

            if (key.MarginTop > 0)
                currentHeight += key.MarginTop;


            if (layoutWidth < currentWidth)
                layoutWidth = currentWidth;

            if (key.LineBreak)
            {
                currentHeight += 37;
                currentWidth = 0;
            }

            if (layoutHeight < currentHeight)
                layoutHeight = currentHeight;
        }

        _region.Width = (float) layoutWidth;
        _region.Height = (float) layoutHeight;
    }

    public void Clear(IEnumerable<KeyboardKey> keyboardKeys)
    {
        OriginRegion = default;
        GroupedKeys.Clear();
        KeyConversion.Clear();
        
        double layoutHeight = 0;
        double layoutWidth = 0;
        double currentHeight = 0;
        double currentWidth = 0;

        foreach (var key in keyboardKeys)
        {
            GroupedKeys.Add(key);

            if (key.Width + key.MarginLeft > 0)
                currentWidth += key.Width + key.MarginLeft;

            if (key.MarginTop > 0)
                currentHeight += key.MarginTop;

            if (layoutWidth < currentWidth)
                layoutWidth = currentWidth;

            if (key.LineBreak)
            {
                currentHeight += 37;
                currentWidth = 0;
            }

            if (layoutHeight < currentHeight)
                layoutHeight = currentHeight;
        }

        _region.Width = (float) layoutWidth;
        _region.Height = (float) layoutHeight;
    }
}