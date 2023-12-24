using System.Windows;

namespace MemoryAccessProfiles.Profiles.ResidentEvil2.Layers;

/// <summary>
/// Interaction logic for Control_ResidentEvil2RankLayer.xaml
/// </summary>
public partial class Control_ResidentEvil2RankLayer
{
    private bool _settingsSet;

    public Control_ResidentEvil2RankLayer()
    {
        InitializeComponent();
    }

    public Control_ResidentEvil2RankLayer(ResidentEvil2RankLayerHandler datacontext)
    {
        InitializeComponent();

        DataContext = datacontext;
    }

    private void SetSettings()
    {
        if (DataContext is ResidentEvil2RankLayerHandler && !_settingsSet)
        {
            _settingsSet = true;
        }
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        SetSettings();

        Loaded -= UserControl_Loaded;
    }
}