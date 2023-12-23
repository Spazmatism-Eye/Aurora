using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aurora.Utils;
using Common.Devices;
using JetBrains.Annotations;

namespace Aurora.Modules.Inputs;

public abstract class AuroraInputEvent : EventArgs
{
    public bool Intercepted { get; set; }
}

public class KeyboardKeyEvent : AuroraInputEvent
{
    public Keys Key { get; }
    private bool IsExtended { get; }
    private DeviceKeys? _deviceKey;

    public KeyboardKeyEvent(Keys key, bool isExtended)
    {
        Key = key;
        IsExtended = isExtended;
    }

    public DeviceKeys GetDeviceKey()
    {
        return _deviceKey ??= KeyUtils.GetDeviceKey(Key, IsExtended);
    }
}

public class MouseKeyEvent : AuroraInputEvent
{
    public MouseButtons Key { get; }

    public MouseKeyEvent(MouseButtons key)
    {
        Key = key;
    }
}

public class MouseScrollEvent : AuroraInputEvent
{
    public int WheelDelta { get; }

    public MouseScrollEvent(int wheelDelta)
    {
        WheelDelta = wheelDelta;
    }
}

public interface IInputEvents : IDisposable
{
    /// <summary>
    /// Event for a Key pressed Down on a keyboard
    /// </summary>
    event EventHandler<KeyboardKeyEvent> KeyDown;

    /// <summary>
    /// Event for a Key released on a keyboard
    /// </summary>
    event EventHandler<KeyboardKeyEvent> KeyUp;

    /// <summary>
    /// Event that fires when a mouse button is pressed down.
    /// </summary>
    event EventHandler<MouseKeyEvent> MouseButtonDown;

    /// <summary>
    /// Event that fires when a mouse button is released.
    /// </summary>
    event EventHandler<MouseKeyEvent> MouseButtonUp;

    /// <summary>
    /// Event that fires when the mouse scroll wheel is scrolled.
    /// </summary>
    event EventHandler<MouseScrollEvent> Scroll;

    IReadOnlyList<Keys> PressedKeys { get; }
    IReadOnlyList<MouseButtons> PressedButtons { get; }
    bool Shift { get; }
    bool Alt { get; }
    bool Control { get; }
    bool Windows { get; }

    [Pure]
    public TimeSpan GetTimeSinceLastInput();
}