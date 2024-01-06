using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aurora.Settings.Layouts;
using Aurora.Utils;
using Common.Devices;

namespace Aurora.Settings.Controls.Keycaps;

/// <summary>
/// Interaction logic for Control_GhostKeycap.xaml
/// </summary>
public partial class Control_GhostKeycap
{
    private readonly bool _isImage;

    public Control_GhostKeycap()
    {
        InitializeComponent();
    }

    public Control_GhostKeycap(KeyboardKey key, string imagePath) : base(key.Tag)
    {
        InitializeComponent();

        Width = key.Width;
        Height = key.Height;

        //Keycap adjustments
        KeyBorder.BorderThickness = new Thickness(string.IsNullOrWhiteSpace(key.Image) ? 1.5 : 0.0);
        KeyBorder.IsEnabled = key.Enabled.Value;

        if (!key.Enabled.Value)
        {
            ToolTipService.SetShowOnDisabled(KeyBorder, true);
            KeyBorder.ToolTip = new ToolTip { Content = "Changes to this key are not supported" };
        }

        if (string.IsNullOrWhiteSpace(key.Image)) return;
        if (!File.Exists(imagePath)) return;

        var memStream = new MemoryStream(File.ReadAllBytes(imagePath));
        var image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = memStream;
        image.EndInit();

        if (key.Tag == DeviceKeys.NONE)
            KeyBorder.Background = new ImageBrush(image);
        else
        {
            KeyBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            KeyBorder.OpacityMask = new ImageBrush(image);
        }

        _isImage = true;
    }

    protected override void DrawColor(Color keyColor)
    {
        keyColor = Color.FromArgb(255, 255, 255, 255); //No colors allowed!

        if (_isImage)
        {
            if (AssociatedKey != DeviceKeys.NONE)
                KeyBorder.Background = new SolidColorBrush(keyColor);
        }

        if (KeyBorder.IsEnabled)
        {
            if (_isImage)
                KeyBorder.Background = new SolidColorBrush(keyColor);
            else
                KeyBorder.Background = new SolidColorBrush(ColorUtils.MultiplyColorByScalar(keyColor, 0.6));
        }
        else
        {
            KeyBorder.Background = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
        }
    }

    private void keyBorder_MouseDown(object? sender, MouseButtonEventArgs e)
    {
        if (sender is Border)
            OnKeySelected();
    }

    private void keyBorder_MouseEnter(object? sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && sender is Border)
            OnKeySelected();
    }
}