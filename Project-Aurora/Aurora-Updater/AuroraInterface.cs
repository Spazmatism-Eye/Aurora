using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora_Updater;

public class AuroraInterface
{
    private readonly byte[] _end = "\n"u8.ToArray();
    
    public async Task ShutdownAurora()
    {
        //gracefully
        var auroraExitTasks = Process.GetProcessesByName("Aurora").Select(p => p.WaitForExitAsync());
        await SendCommand("shutdown");
        
        await Task.WhenAny(Task.Delay(2), Task.WhenAll(auroraExitTasks));
        
        //forcefully
        foreach (var proc in Process.GetProcessesByName("Aurora"))
            proc.Kill();
    }

    private Task SendCommand(string command)
    {
        return SendCommand(Encoding.UTF8.GetBytes(command));
    }

    private async Task SendCommand(byte[] command)
    {
        var client = new NamedPipeClientStream(".", "aurora\\interface", PipeDirection.Out, PipeOptions.Asynchronous);
        await client.ConnectAsync(2000);
        if (!client.IsConnected)
            return;
        
        client.Write(command, 0, command.Length);
        client.Write(_end, 0, _end.Length);
        
        client.Flush();
        client.Close();
    }
}