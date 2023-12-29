using System.Windows;
using Aurora.Modules;
using Aurora.Modules.Inputs;
using Keys = System.Windows.Forms.Keys;

namespace Aurora.Controls;

/// <summary>
/// Interaction logic for Control_SingleKeyEditor.xaml
/// </summary>
public partial class Control_SingleKeyEditor
{

    /// <summary>A reference to the editor that is currently listening for a keypress</summary>
    private static Control_SingleKeyEditor? _listeningEditor;

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

    private static void InputEvents_KeyDown(object? sender, KeyboardKeyEvent e) {
        if (_listeningEditor != null)
            _listeningEditor.Dispatcher.Invoke(() => {
                _listeningEditor.SelectedKey = e.Key;
                _listeningEditor.UpdateButtonText(false);
                _listeningEditor = null;
            });
    }

    // Dependency Property
    public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register("SelectedKey", typeof(Keys), typeof(Control_SingleKeyEditor), new FrameworkPropertyMetadata(default(Keys), FrameworkPropertyMetadataOptions.AffectsRender));

    public Keys SelectedKey {
        get => (Keys)GetValue(SelectedKeyProperty);
        set => SetValue(SelectedKeyProperty, value);
    }

    private async void Control_SingleKeyEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
        (await InputsModule.InputEvents).KeyDown += InputEvents_KeyDown;
    }
}