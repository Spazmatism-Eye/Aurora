using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Aurora.Settings;

namespace Aurora.Controls;

/// <summary>
/// Interaction logic for KeyBindList.xaml
/// </summary>
public partial class KeyBindList
{
    public delegate void ChangeHandler(object? sender);
    /// <summary>Triggered when a keybind is updated, created or deleted.</summary>
    public event ChangeHandler? KeybindsChanged;

    private List<Keybind> _keybinds = [];

    public KeyBindList() {
        InitializeComponent();
    }

    /// <summary>Gets or sets the list of keybinds that this control is representing.</summary>
    public Keybind[] Keybinds {
        get => _keybinds.ToArray();
        set {
            _keybinds = [..value];
            PopulateList();
        }
    }

    /// <summary>Clears and populates the UI list based on the keybinds list.</summary>
    private void PopulateList() {
        keyList.Children.Clear();
        foreach (var keybind in _keybinds)
            CreateShortcutItem(keybind);
    }

    /// <summary>Triggered when the add new button is pressed.</summary>
    private void addNew_Click(object? sender, RoutedEventArgs e) {
        // Create a keybind object and a shortcut item to represent it
        var k = new Keybind();
        _keybinds.Add(k);
        var s = CreateShortcutItem(k);

        TriggerKeybindChange(s, k); // Trigger a change event (since there is a new item)
    }

    /// <summary>Creates a new keybind/shortcut list item and assigns the relevant listeners.</summary>
    /// <param name="k">Keybind that the shortcut item will represent.</param>
    private ShortcutItem CreateShortcutItem(Keybind k) {
        var s = new ShortcutItem(k);
        keyList.Children.Add(s);
        s.KeybindChange += TriggerKeybindChange;
        s.DeleteTriggered += DeleteKeybind;
        return s;
    }

    /// <summary>Handles a shortcut item's delete button being pressed.</summary>
    private void DeleteKeybind(ShortcutItem sender, Keybind keybindTarget) {
        _keybinds.Remove(keybindTarget); // delete keybind
        keyList.Children.Remove(sender); // delete UI representing keybind

        TriggerKeybindChange(sender, keybindTarget); // Trigger a change event
    }

    /// <summary>Handler for keybind's change event and can be called manually to fire a change event.</summary>
    private void TriggerKeybindChange(ShortcutItem item, Keybind @new) {
        KeybindsChanged?.Invoke(this);
    }
}

class ShortcutItem : DockPanel {
    public delegate void DeleteHandler(ShortcutItem sender, Keybind keybindTarget);
    public event DeleteHandler? DeleteTriggered;

    public delegate void ChangeHandler(ShortcutItem sender, Keybind keybindTarget);
    public event ChangeHandler? KeybindChange;

    public Keybind Keybind { get; }

    private readonly Control_Keybind _kb;
    private readonly Button _delete;

    public ShortcutItem(Keybind keybind)
    {
        Keybind = keybind;

        HorizontalAlignment = HorizontalAlignment.Stretch;
        LastChildFill = true;

        _delete = new Button { Content = "X" };
        _delete.Click += RaiseDelete;
        SetDock(_delete, Dock.Right);
        Children.Add(_delete);

        _kb = new Control_Keybind { ContextKeybind = keybind };
        _kb.KeybindUpdated += RaiseChange;
        Children.Add(_kb);
    }

    private void RaiseDelete(object? sender, RoutedEventArgs e) {
        DeleteTriggered?.Invoke(this, Keybind);
    }

    private void RaiseChange(object? sender, Keybind @new) {
        KeybindChange?.Invoke(this, Keybind);
    }
}