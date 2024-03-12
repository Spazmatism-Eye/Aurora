using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AuroraRgb.Modules.Inputs;

public sealed class NoopInputEvents : IInputEvents
{
    private static readonly TimeSpan NoopLastInput = new(0);

    public void Dispose()
    {
        //noop
    }

    public event EventHandler<KeyboardKeyEventArgs>? KeyDown;
    public event EventHandler<KeyboardKeyEventArgs>? KeyUp;
    public event EventHandler<MouseKeyEventArgs>? MouseButtonDown;
    public event EventHandler<MouseKeyEventArgs>? MouseButtonUp;
    public event EventHandler<MouseScrollEventArgs>? Scroll;
    public IReadOnlyCollection<Keys> PressedKeys { get; } = new HashSet<Keys>();
    public IReadOnlyCollection<MouseButtons> PressedMouseButtons { get; } = new HashSet<MouseButtons>();
    public bool Shift => false;
    public bool Alt => false;
    public bool Control => false;
    public bool Windows => false;

    public TimeSpan GetTimeSinceLastInput() {
        return NoopLastInput;
    }
}