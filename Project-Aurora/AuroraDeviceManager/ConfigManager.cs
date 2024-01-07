using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using AuroraDeviceManager.AuroraMigration;
using AuroraDeviceManager.Devices;
using AuroraDeviceManager.Utils;
using Common;
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
            config = await CreateDefaultConfigurationFile();
        else
        {
            var content = await File.ReadAllTextAsync(ConfigFile, Encoding.UTF8);
            config = string.IsNullOrWhiteSpace(content)
                ? await CreateDefaultConfigurationFile()
                : JsonSerializer.Deserialize(content, CommonSourceGenerationContext.Default.DeviceConfig) ??
                  await CreateDefaultConfigurationFile();
        }

        config.OnPostLoad();

        Global.DeviceConfig = config;
        deviceManager.RegisterVariables();
    }

    private async Task<DeviceConfig> CreateDefaultConfigurationFile()
    {
        var auroraConfigFile = Path.Combine(Global.AppDataDirectory, "Config.json.v194");
        if (!File.Exists(auroraConfigFile))
        {
            return new DeviceConfig();
        }

        var content = await File.ReadAllTextAsync(auroraConfigFile, Encoding.UTF8);
        if (string.IsNullOrWhiteSpace(content))
            return new DeviceConfig();

        var auroraConfig = JsonSerializer.Deserialize(content, AuroraSourceGenerationContext.Default.AuroraConfiguration);
        if (auroraConfig == null) return new DeviceConfig();
        
        Global.Logger.Information("Migrating DeviceConfig.json");

        var varRegistryVariables = auroraConfig.VarRegistry.Variables
            .Where(pair => pair.Value.GetValueKind() == JsonValueKind.Object)
            .ToDictionary(pair => pair.Key, pair => pair.Value.Deserialize(SourceGenerationContext.Default.VariableRegistryItem)!);

        var migratedConfig = new DeviceConfig
        {
            EnabledDevices = new ObservableCollection<string>(auroraConfig.EnabledDevices.Values),
            DeviceCalibrations = new Dictionary<string, SimpleColor>(auroraConfig.DeviceCalibrations.Values),
            AllowPeripheralDevices = auroraConfig.AllowPeripheralDevices,
            DevicesDisableHeadset = auroraConfig.DevicesDisableHeadset,
            DevicesDisableKeyboard = auroraConfig.DevicesDisableKeyboard,
            DevicesDisableMouse = auroraConfig.DevicesDisableMouse,
            VarRegistry = new VariableRegistry { Variables = varRegistryVariables }
        };
        Save(migratedConfig, DeviceConfig.ConfigFile);
        try
        {
            File.Delete(auroraConfigFile);
        }catch{ /* ignore */ }
        return migratedConfig;
    }

    private static void Save(object configuration, string path)
    {
        var content = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        File.WriteAllText(path, content, Encoding.UTF8);
    }
}