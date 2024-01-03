using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Aurora_Updater;

public class UpdaterConfiguration(bool getDevReleases)
{
    private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aurora", "Config");
    private const string ConfigExtension = ".json";
    
    public bool GetDevReleases { get; } = getDevReleases;

    public static UpdaterConfiguration Load()
    {
        UpdaterConfiguration config;
        try
        {
            config = TryLoad();
        }
        catch (Exception)
        {
            config = new UpdaterConfiguration(false);
        }

        return config;
    }
    
    private static UpdaterConfiguration TryLoad()
    {
        UpdaterConfiguration config;
        var configPath = ConfigPath + ConfigExtension;

        if (!File.Exists(configPath))
            config = new UpdaterConfiguration(false);
        else
        {
            var content = File.ReadAllText(configPath, Encoding.UTF8);
            config = string.IsNullOrWhiteSpace(content)
                ? new UpdaterConfiguration(false)
                : JsonSerializer.Deserialize<UpdaterConfiguration>(content)!;
        }

        return config;
    }
}