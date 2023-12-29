using System;
using System.Windows.Controls;
using System.Windows.Media;
using Aurora.Utils;
using Common.Devices;

namespace Aurora.Settings.Controls.Keycaps;

public abstract class Keycap : UserControl
{
    protected static readonly SolidColorBrush DefaultColorBrush = new (Color.FromArgb(255, 0, 0, 0));

    protected readonly DeviceKeys AssociatedKey = DeviceKeys.NONE;

    private Color _currentColor = Color.FromArgb(0, 0, 0, 0);

    static Keycap()
    {
        DefaultColorBrush.Freeze();
    }

    protected Keycap()
    {
    }

    protected Keycap(DeviceKeys associatedKey)
    {
        AssociatedKey = associatedKey;
    }

    public void SetColor(Color keyColor)
    {
        if (Global.key_recorder?.HasRecorded(AssociatedKey) ?? false)
        {
            var g = (byte)(Math.Min(Math.Pow(Math.Cos(Time.GetMilliSeconds() / 1000.0 * Math.PI) + 0.05, 2.0), 1.0) * 255);
            var glowColor = Color.FromArgb(255, 0, g, 0);
            if (glowColor.Equals(_currentColor))
            {
                return;
            }
            DrawColor(glowColor);
            _currentColor = keyColor;
            return;
        }

        if (keyColor.Equals(_currentColor))
        {
            return;
        }
        
        DrawColor(keyColor);

        _currentColor = keyColor;
    }

    protected void OnKeySelected()
    {
        if (AssociatedKey == DeviceKeys.NONE || Global.key_recorder == null) return;
        if (Global.key_recorder.HasRecorded(AssociatedKey))
            Global.key_recorder.RemoveKey(AssociatedKey);
        else
            Global.key_recorder.AddKey(AssociatedKey);
    }

    protected abstract void DrawColor(Color keyColor);

    public DeviceKeys GetKey()
    {
        return AssociatedKey;
    }
}