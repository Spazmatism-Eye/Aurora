using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using AuroraRgb.Devices;
using AuroraRgb.Settings;
using Newtonsoft.Json;

namespace AuroraRgb.Profiles.Chroma;

[JsonObject]
public class ChromaApplicationSettings: ApplicationSettings
{
    public ObservableCollection<string> ExcludedPrograms { get; } = new();
    
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        if (!ExcludedPrograms.Contains(Global.AuroraExe))
        {
            ExcludedPrograms.Add(Global.AuroraExe);
        }
        if (!ExcludedPrograms.Contains(DeviceManager.DeviceManagerExe))
        {
            ExcludedPrograms.Add(DeviceManager.DeviceManagerExe);
        }
    }
}