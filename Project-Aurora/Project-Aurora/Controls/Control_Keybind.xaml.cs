﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AuroraRgb.Modules;
using AuroraRgb.Modules.Inputs;
using AuroraRgb.Settings;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace AuroraRgb.Controls;

/// <summary>
/// Interaction logic for Control_Keybind.xaml
/// </summary>
public partial class Control_Keybind
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public static readonly DependencyProperty KeybindProperty = DependencyProperty.Register(
        "Keybind", typeof(Keybind), typeof(Control_Keybind));

    public Keybind? ContextKeybind
    {
        get => (Keybind)GetValue(KeybindProperty);
        set
        {
            SetValue(KeybindProperty, value);

            if (value == null) return;
            textBoxKeybind.Text = value.ToString();
            KeybindUpdated?.Invoke(this, value);
        }
    }

    public delegate void NewKeybindArgs(object? sender, Keybind newKeybind);

    public event NewKeybindArgs? KeybindUpdated;

    public static Control_Keybind? _ActiveKeybind { get; private set; } //Makes sure that only one keybind can be set at a time

    public Control_Keybind()
    {
        InitializeComponent();
    }

    private void InputEventsKeyDown(object? sender, KeyboardKeyEventArgs e)
    {
        Dispatcher.BeginInvoke(() =>
            {
                if (!Equals(_ActiveKeybind)) return;
                var pressedKeys = e.PressedKeys;

                if (ContextKeybind != null)
                {
                    ContextKeybind.SetKeys(pressedKeys);
                    textBoxKeybind.Text = ContextKeybind.ToString();
                    KeybindUpdated?.Invoke(this, ContextKeybind);
                }
                else
                    textBoxKeybind.Text = "ERROR (No KeybindProperty set)";
            }, DispatcherPriority.Input);
    }

    private bool _isRecording;
    public void Start()
    {
        if (_ActiveKeybind != null)
            _ActiveKeybind.Stop();

        _isRecording = true;
        _ActiveKeybind = this;

        buttonToggleAssign.Content = "Stop";
    }

    public void Stop()
    {
        buttonToggleAssign.Content = "Assign";
        _isRecording = false;
        _ActiveKeybind = null;
        KeybindUpdated?.Invoke(this, ContextKeybind);
    }

    private void buttonToggleAssign_Click(object? sender, RoutedEventArgs e)
    {
        if (_isRecording)
            Stop();
        else
            Start();
    }

    private void Grid_LostFocus(object? sender, RoutedEventArgs e)
    {
        Stop();
    }

    private void Grid_KeyDown(object? sender, KeyEventArgs e)
    {
        if ((e.Key.Equals(Key.Down) || e.Key.Equals(Key.Up) || e.Key.Equals(Key.Left) || e.Key.Equals(Key.Right)) && _isRecording)
            e.Handled = true;
    }

    private async void Control_Keybind_OnLoaded(object sender, RoutedEventArgs e)
    {
        (await InputsModule.InputEvents).KeyDown += InputEventsKeyDown;
    }

    private async void Control_Keybind_OnUnloaded(object sender, RoutedEventArgs e)
    {
        (await InputsModule.InputEvents).KeyDown -= InputEventsKeyDown;
    }
}