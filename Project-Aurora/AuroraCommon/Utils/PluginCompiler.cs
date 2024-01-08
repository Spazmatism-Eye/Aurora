using System.Diagnostics;
using Serilog;

namespace Common.Utils;

public class PluginCompiler(ILogger logger, string path)
{
    public async Task Compile(string scriptPath)
    {
        var scriptChangeTime = File.GetLastWriteTime(scriptPath);
        var dllFile = scriptPath + ".dll";
        var dllCompileTime = File.Exists(dllFile) ? File.GetLastWriteTime(dllFile) : DateTime.UnixEpoch;

        if (scriptChangeTime < dllCompileTime)
        {
            logger.Information("Script {Script} is up to date", scriptPath);
            return;
        }

        logger.Information("Compiling script: {Script}", scriptPath);
        var compilerPath = Path.Combine(path, "PluginCompiler.exe");
        var compilerProc = new ProcessStartInfo
        {
            WorkingDirectory = Path.GetDirectoryName(Environment.ProcessPath),
            FileName = compilerPath,
            Arguments = scriptPath,
        };
        var process = Process.Start(compilerProc);
        if (process == null)
        {
            throw new ApplicationException("PluginCompiler.exe not found!");
        }

        process.ErrorDataReceived += (_, args) =>
        {
            logger.Error("Compiler: {}", args.Data);
        };

        try
        {
            await process.WaitForExitAsync();
        }
        catch (Exception e)
        {
            logger.Error(e, "Could not load script: {Script}", scriptPath);
        }
    }
}