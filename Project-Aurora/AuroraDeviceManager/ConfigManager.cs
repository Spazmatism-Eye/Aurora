using System.Text;
using System.Text.Json;
using AuroraDeviceManager.Devices;
using AuroraDeviceManager.Utils;
using Common.Devices;
using Common.Utils;

namespace AuroraDeviceManager;

public class ConfigManager(DeviceManager deviceManager)
{
    private static readonly string ConfigFile = Path.Combine(Global.AppDataDirectory, DeviceConfig.FileName);

    private static FileSystemWatcher? _configFileWatcher;

    public async Task Load()
    {
        _configFileWatcher = new FileSystemWatcher(Global.AppDataDirectory)
        {
            Filter = "Config.json",
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };
        await TryLoad();
        
        _configFileWatcher.Changed += ConfigFileWatcherOnChanged;
    }

    private void ConfigFileWatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        Thread.Sleep(200);
        try
        {
            TryLoad().Wait();
        }
        catch (Exception exc)
        {
            Global.Logger.Error(exc, "Failed to load configuration");
        }
    }

    private async Task TryLoad()
    {
        DeviceConfig config;

        if (!File.Exists(ConfigFile))
            config = CreateDefaultConfigurationFile();
        else
        {
            var content = await File.ReadAllTextAsync(ConfigFile, Encoding.UTF8);
            config = string.IsNullOrWhiteSpace(content)
                ? CreateDefaultConfigurationFile()
                : JsonSerializer.Deserialize(content, CommonSourceGenerationContext.Default.DeviceConfig) ?? CreateDefaultConfigurationFile();
        }
        config.OnPostLoad();

        Global.DeviceConfig = config;
        deviceManager.RegisterVariables();
    }

    private static DeviceConfig CreateDefaultConfigurationFile()
    {
        return new DeviceConfig();
    }
}