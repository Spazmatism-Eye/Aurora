using System.Windows;

namespace MemoryAccessProfiles.Profiles.ResidentEvil2.Layers;

/// <summary>
/// Interaction logic for Control_ResidentEvil2HealthLayer.xaml
/// </summary>
public partial class Control_ResidentEvil2HealthLayer
{
    private bool settingsset;

    public Control_ResidentEvil2HealthLayer()
    {
        InitializeComponent();
    }

    public Control_ResidentEvil2HealthLayer(ResidentEvil2HealthLayerHandler datacontext)
    {
        InitializeComponent();

        DataContext = datacontext;
    }

    public void SetSettings()
    {
        if (DataContext is ResidentEvil2HealthLayerHandler && !settingsset)
        {
            if (!status_style.HasItems)
            {
                status_style.Items.Add(ResidentEvil2HealthLayerHandlerProperties.HealthDisplayType.Static);
                status_style.Items.Add(ResidentEvil2HealthLayerHandlerProperties.HealthDisplayType.Scanning);
            }

            status_style.SelectedItem = (DataContext as ResidentEvil2HealthLayerHandler).Properties._DisplayType ?? ResidentEvil2HealthLayerHandlerProperties.HealthDisplayType.Static;

            settingsset = true;
        }
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        SetSettings();

        Loaded -= UserControl_Loaded;
    }

    private void status_style_SelectionChanged(object? sender, RoutedEventArgs e)
    {
        if (IsLoaded && settingsset && DataContext is ResidentEvil2HealthLayerHandler)
        {
            (DataContext as ResidentEvil2HealthLayerHandler).Properties._DisplayType = (ResidentEvil2HealthLayerHandlerProperties.HealthDisplayType)status_style.SelectedItem;
        }
    }
}