using System.Threading.Tasks;
using System.Windows;
using Aurora.Modules.ProcessMonitor;
using Lombok.NET;

namespace Aurora.Modules;

public sealed partial class ProcessesModule : AuroraModule
{
    public static Task<ActiveProcessMonitor> ActiveProcessMonitor => ActiveProcess.Task;
    public static Task<RunningProcessMonitor> RunningProcessMonitor => RunningProcess.Task;

    private static readonly TaskCompletionSource<ActiveProcessMonitor> ActiveProcess = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static readonly TaskCompletionSource<RunningProcessMonitor> RunningProcess = new(TaskCreationOptions.RunContinuationsAsynchronously);
    
    protected override Task Initialize()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            ActiveProcess.SetResult(new ActiveProcessMonitor());
            RunningProcess.SetResult(new RunningProcessMonitor());
        });
        return Task.CompletedTask;
    }


    [Async]
    public override void Dispose()
    {
        if (ActiveProcessMonitor.IsCompletedSuccessfully)
        {
            ActiveProcessMonitor.Result.Dispose();
        }
        if (RunningProcessMonitor.IsCompletedSuccessfully)
        {
            RunningProcessMonitor.Result.Dispose();
        }
    }
}