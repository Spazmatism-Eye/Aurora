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

public sealed class ConfigManager(DeviceManager deviceManager): IDisposable
{
    private static readonly string ConfigFile = Path.Combine(Global.AppDataDirectory, DeviceConfig.FileName);

    private FileSystemWatcher? _configFileWatcher;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
    };

    public async Task Load()
    {
        _configFileWatcher = new FileSystemWatcher(Global.AppDataDirectory)
        {
            Filter = DeviceConfig.FileName,
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };
        await TryLoad();

        _configFileWatcher.Changed += ConfigFileWatcherOnChanged;
    }

    public void Dispose()
    {
        _configFileWatcher?.Dispose();
    }

    private void ConfigFileWatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        Thread.Sleep(200);
        try
        {
            TryLoad(false).Wait();
        }
        catch (Exception exc)
        {
            Global.Logger.Error(exc, "Failed to load configuration");
        }
    }

    private async Task TryLoad(bool save = true)
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
        if (save)
        {
            await Save(config, DeviceConfig.ConfigFile);
        }
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
        await Save(migratedConfig, DeviceConfig.ConfigFile);
        try
        {
            File.Delete(auroraConfigFile);
        }catch{ /* ignore */ }
        return migratedConfig;
    }

    private static Task Save(object configuration, string path)
    {
        var content = JsonSerializer.Serialize(configuration, JsonSerializerOptions);

        return File.WriteAllTextAsync(path, content, Encoding.UTF8);
    }
}