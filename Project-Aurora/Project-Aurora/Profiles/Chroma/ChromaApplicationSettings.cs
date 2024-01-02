using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Aurora.Settings;
using Newtonsoft.Json;

namespace Aurora.Profiles.Chroma;

[JsonObject]
public class ChromaApplicationSettings: ApplicationSettings
{
    public ObservableCollection<string> ExcludedPrograms { get; } = new();
    
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        if (!ExcludedPrograms.Contains("Aurora.exe"))
        {
            ExcludedPrograms.Add("Aurora.exe");
        }
        if (!ExcludedPrograms.Contains("AuroraDeviceManager.exe"))
        {
            ExcludedPrograms.Add("AuroraDeviceManager.exe");
        }
    }
}