using System.Windows;
using System.Windows.Threading;
using AuroraRgb.Modules;
using AuroraRgb.Modules.Inputs;
using Keys = System.Windows.Forms.Keys;

namespace AuroraRgb.Controls;

/// <summary>
/// Interaction logic for Control_SingleKeyEditor.xaml
/// </summary>
public partial class Control_SingleKeyEditor
{

    /// <summary>A reference to the editor that is currently listening for a keypress</summary>
    private Control_SingleKeyEditor? _listeningEditor;

    // Static constructor so that we only have to add a input event listener once.
    static Control_SingleKeyEditor() {
    }

    // Instance constructor to create UI elements
    public Control_SingleKeyEditor() {
        InitializeComponent();
        DataContext = this;
    }

    // Assign or unassign the `listeningEditor` from this UserControl
    private void AssignButton_Click(object? sender, RoutedEventArgs e) {
        var assigning = _listeningEditor != this;
        _listeningEditor?.UpdateButtonText(false);
        UpdateButtonText(assigning);
        _listeningEditor = assigning ? this : null;
    }

    private void UpdateButtonText(bool assigning) {
        assignButton.Content = assigning ? "Press a key" : "Assign";
    }

    private void InputEvents_KeyDown(object? sender, KeyboardKeyEventArgs e)
    {
        _listeningEditor?.Dispatcher.BeginInvoke(() => {
            _listeningEditor.SelectedKey = e.Key;
            _listeningEditor.UpdateButtonText(false);
            _listeningEditor = null;
        }, DispatcherPriority.Input);
    }

    // Dependency Property
    public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register(nameof(SelectedKey), typeof(Keys),
        typeof(Control_SingleKeyEditor), new FrameworkPropertyMetadata(default(Keys), FrameworkPropertyMetadataOptions.AffectsRender));

    public Keys SelectedKey {
        get => (Keys)GetValue(SelectedKeyProperty);
        set => SetValue(SelectedKeyProperty, value);
    }

    private async void Control_SingleKeyEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
        (await InputsModule.InputEvents).KeyDown += InputEvents_KeyDown;
    }

    private async void Control_SingleKeyEditor_OnUnloaded(object sender, RoutedEventArgs e)
    {
        (await InputsModule.InputEvents).KeyDown -= InputEvents_KeyDown;
    }
}