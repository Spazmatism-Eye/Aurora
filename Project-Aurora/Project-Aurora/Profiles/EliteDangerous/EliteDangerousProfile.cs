using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;

namespace AuroraRgb.Profiles.EliteDangerous;

public class EliteDangerousProfile : ApplicationProfile
{
    public override void Reset()
    {
        base.Reset();
        Layers = new System.Collections.ObjectModel.ObservableCollection<Layer>()
        {
            new("Animations", new Layers.EliteDangerousAnimationLayerHandler()),
            new("Key Binds", new Layers.EliteDangerousKeyBindsLayerHandler()),
            new("Background", new Layers.EliteDangerousBackgroundLayerHandler()),
        };
    }
}