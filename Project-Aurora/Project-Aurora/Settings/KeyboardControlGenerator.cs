using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aurora.Settings.Controls.Keycaps;
using Aurora.Settings.Layouts;
using Common.Devices;

namespace Aurora.Settings;

internal class KeyboardControlGenerator(
    bool abstractKeycaps,
    VirtualGroup virtualKeyboardGroup,
    IDictionary<DeviceKeys, Keycap> virtualKeyboardMap,
    string layoutsPath,
    Grid virtualKeyboard)
{
    private double _layoutHeight;
    private double _layoutWidth;

    private double _currentHeight;
    private double _currentWidth;

    public double BaselineX { get; private set; }
    public double BaselineY { get; private set; }

    internal async Task<Grid> Generate()
    {
        var imagesPath = Path.Combine(layoutsPath, "Extra Features", "images");

        virtualKeyboard.Children.Clear();
        foreach (var key in virtualKeyboardGroup.GroupedKeys.OrderBy(a => a.ZIndex))
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { CreateKey(key, imagesPath); });
        }

        if (virtualKeyboardGroup.GroupedKeys.Count == 0)
        {
            //No items, display error
            var errorMessage = new Label();

            var infoPanel = new DockPanel();

            var infoMessage = new TextBlock
            {
                Text = "No keyboard selected\r\nPlease select your keyboard in the settings",
                TextAlignment = TextAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
            };

            DockPanel.SetDock(infoMessage, Dock.Top);
            infoPanel.Children.Add(infoMessage);

            var infoInstruction = new DockPanel();

            infoInstruction.Children.Add(new TextBlock
            {
                Text = "Press (",
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                VerticalAlignment = VerticalAlignment.Center
            });

            infoInstruction.Children.Add(new Image
            {
                Source = new BitmapImage(new Uri(@"Resources/settings_icon.png", UriKind.Relative)),
                Stretch = Stretch.Uniform,
                Height = 40.0,
                VerticalAlignment = VerticalAlignment.Center
            });

            infoInstruction.Children.Add(new TextBlock
            {
                Text = ") and go into \"Devices & Wrappers\" tab",
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                VerticalAlignment = VerticalAlignment.Center
            });

            DockPanel.SetDock(infoInstruction, Dock.Bottom);
            infoPanel.Children.Add(infoInstruction);

            errorMessage.Content = infoPanel;

            errorMessage.FontSize = 16.0;
            errorMessage.FontWeight = FontWeights.Bold;
            errorMessage.HorizontalContentAlignment = HorizontalAlignment.Center;
            errorMessage.VerticalContentAlignment = VerticalAlignment.Center;

            virtualKeyboard.Children.Add(errorMessage);

            //Update size
            virtualKeyboard.Width = 850;
            virtualKeyboard.Height = 200;
        }
        else
        {
            //Update size
            virtualKeyboard.Width = virtualKeyboardGroup.Region.Width;
            virtualKeyboard.Height = virtualKeyboardGroup.Region.Height;
        }

        return virtualKeyboard;
    }

    private void CreateKey(KeyboardKey key, string imagesPath)
    {
        var keyMarginLeft = key.MarginLeft;
        var keyMarginTop = key.MarginTop;

        var imagePath = "";

        if (!string.IsNullOrWhiteSpace(key.Image))
            imagePath = Path.Combine(imagesPath, key.Image);

        UserControl keycap;

        //Ghost keycap is used for abstract representation of keys
        if (abstractKeycaps)
            keycap = new Control_GhostKeycap(key, imagePath);
        else
        {
            switch (Global.Configuration.VirtualkeyboardKeycapType)
            {
                case KeycapType.Default_backglow:
                    keycap = new Control_DefaultKeycapBackglow(key, imagePath);
                    break;
                case KeycapType.Default_backglow_only:
                    keycap = new Control_DefaultKeycapBackglowOnly(key, imagePath);
                    break;
                case KeycapType.Colorized:
                    keycap = new Control_ColorizedKeycap(key, imagePath);
                    break;
                case KeycapType.Colorized_blank:
                    keycap = new Control_ColorizedKeycapBlank(key, imagePath);
                    break;
                default:
                    keycap = new Control_DefaultKeycap(key, imagePath);
                    break;
            }
        }

        virtualKeyboard.Children.Add(keycap);

        if (key.Tag != DeviceKeys.NONE && !virtualKeyboardMap.ContainsKey(key.Tag) && keycap is Keycap kc && !abstractKeycaps)
            virtualKeyboardMap.Add(key.Tag, kc);

        if (key.AbsoluteLocation)
            keycap.Margin = new Thickness(key.MarginLeft, key.MarginTop, 0, 0);
        else
            keycap.Margin = new Thickness(_currentWidth + key.MarginLeft, _currentHeight + key.MarginTop, 0, 0);

        if (key.Tag == DeviceKeys.ESC)
        {
            BaselineX = keycap.Margin.Left;
            BaselineY = keycap.Margin.Top;
        }

        if (key.AbsoluteLocation) return;
        if (key.Width + keyMarginLeft > 0)
            _currentWidth += key.Width + keyMarginLeft;

        if (keyMarginTop > 0)
            _currentHeight += keyMarginTop;


        if (_layoutWidth < _currentWidth)
            _layoutWidth = _currentWidth;

        if (key.LineBreak)
        {
            _currentHeight += 37;
            _currentWidth = 0;
        }

        if (_layoutHeight < _currentHeight)
            _layoutHeight = _currentHeight;
    }
}