using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lombok.NET;

namespace Aurora.Modules;

public partial class UpdateCleanup : AuroraModule
{
    protected override Task Initialize()
    {
        if (!Global.Configuration.Migrations.Contains("net8"))
        {
            Net8Migration();
            Global.Configuration.Migrations.Add("net8");
        }
        CleanOldLogiDll();
        CleanLogs();
        return Task.CompletedTask;
    }

    private static void CleanOldLogiDll()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Directory.SetCurrentDirectory(path);
        var logiDll = Path.Combine(path, "LogitechLed.dll");
        if (File.Exists(logiDll))
            File.Delete(logiDll);
    }

    private static void CleanLogs()
    {
        var logFolder = Path.Combine(Global.AppDataDirectory, "Logs");
        //TODO regex match
        var logFile = new Regex(".*\\.log");
        var files = from file in Directory.EnumerateFiles(logFolder)
            where logFile.IsMatch(Path.GetFileName(file))
            orderby File.GetCreationTime(file) descending
            select file;
        foreach (var file in files.Skip(8))
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Global.logger.Error(e, "Failed to delete log: {File}", file);
            }
        }
    }

    [Async]
    public override void Dispose()
    {
        //noop
    }

    private void Net8Migration()
    {
        IEnumerable<string> files = [
        "Accessibility.dl",
        "clrcompression.dl",
        "D3DCompiler_47_cor3.dl",
        "DirectWriteForwarder.dl",
        "Microsoft.VisualBasic.Forms.dl",
        "Microsoft.Win32.Registry.AccessControl.dl",
        "Microsoft.Win32.SystemEvents.dl",
        "System.Windows.Controls.Ribbon.dl",
        "System.Windows.Extensions.dl",
        "System.Windows.Forms.Design.dl",
        "System.Windows.Forms.Design.Editors.dl",
        "System.Windows.Forms.dl",
        "System.Windows.Forms.Primitives.dl",
        "System.Windows.Input.Manipulations.dl",
        "System.Windows.Presentation.dl",
        "System.Xaml.dl",
        "WindowsFormsIntegration.dl",
        "wpfgfx_cor3.dll"
        ];

        foreach (var file in files)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}