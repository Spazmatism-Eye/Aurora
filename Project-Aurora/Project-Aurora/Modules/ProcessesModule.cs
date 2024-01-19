using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aurora.Modules.ProcessMonitor;
using Lombok.NET;

namespace Aurora.Modules;

public sealed partial class ProcessesModule : AuroraModule
{
    public static Task<ActiveProcessMonitor> ActiveProcessMonitor => ActiveProcess.Task;
    public static Task<RunningProcessMonitor> RunningProcessMonitor => RunningProcess.Task;

    private static readonly TaskCompletionSource<ActiveProcessMonitor> ActiveProcess = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static readonly TaskCompletionSource<RunningProcessMonitor> RunningProcess = new(TaskCreationOptions.RunContinuationsAsynchronously);
    
    protected override async Task Initialize()
    {
        await Application.Current.Dispatcher.BeginInvoke(() =>
        {
            ActiveProcess.SetResult(new ActiveProcessMonitor());
            RunningProcess.SetResult(new RunningProcessMonitor());
        }, DispatcherPriority.Send);
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