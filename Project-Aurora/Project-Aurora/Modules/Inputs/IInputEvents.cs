using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AuroraRgb.Utils;
using Common.Devices;
using JetBrains.Annotations;

namespace AuroraRgb.Modules.Inputs;

public abstract class AuroraInputEventArgs : EventArgs
{
    public bool Intercepted { get; set; }
}

public class KeyboardKeyEventArgs(Keys key, bool isExtended, IReadOnlyCollection<Keys> pressedKeys) : AuroraInputEventArgs
{
    public Keys Key { get; } = key;
    private bool IsExtended { get; } = isExtended;
    public IReadOnlyCollection<Keys> PressedKeys { get; } = pressedKeys;

    public DeviceKeys GetDeviceKey()
    {
        return KeyUtils.GetDeviceKey(Key, IsExtended);
    }
}

public class MouseKeyEventArgs(MouseButtons key) : AuroraInputEventArgs
{
    public MouseButtons Key { get; } = key;
}

public class MouseScrollEventArgs(int wheelDelta) : AuroraInputEventArgs
{
    public int WheelDelta { get; } = wheelDelta;
}

public interface IInputEvents : IDisposable
{
    /// <summary>
    /// Event for a Key pressed Down on a keyboard
    /// </summary>
    event EventHandler<KeyboardKeyEventArgs> KeyDown;

    /// <summary>
    /// Event for a Key released on a keyboard
    /// </summary>
    event EventHandler<KeyboardKeyEventArgs> KeyUp;

    /// <summary>
    /// Event that fires when a mouse button is pressed down.
    /// </summary>
    event EventHandler<MouseKeyEventArgs> MouseButtonDown;

    /// <summary>
    /// Event that fires when a mouse button is released.
    /// </summary>
    event EventHandler<MouseKeyEventArgs> MouseButtonUp;

    /// <summary>
    /// Event that fires when the mouse scroll wheel is scrolled.
    /// </summary>
    event EventHandler<MouseScrollEventArgs> Scroll;

    IReadOnlyCollection<Keys> PressedKeys { get; }
    IReadOnlyCollection<MouseButtons> PressedMouseButtons { get; }
    bool Shift { get; }
    bool Alt { get; }
    bool Control { get; }
    bool Windows { get; }

    [Pure]
    public TimeSpan GetTimeSinceLastInput();
}