using Amib.Threading;

namespace Common.Utils;

/**
 * Makes sure only one thread is running the Action.
 * Makes sure last trigger is invoked after the call
 */
public class SingleConcurrentThread
{
    private readonly SmartThreadPool _worker = new(1000, 1)
    {
        Concurrency = 1,
        MaxQueueLength = 1
    };
    
    private readonly Action _updateAction;

    public SingleConcurrentThread(string threadName, Action updateAction)
    {
        _updateAction = updateAction;
        _worker.Name = threadName;
    }

    public void Trigger()
    {
        // (_worker.CurrentWorkItemsCount == 0 || _worker.InUseThreads == 0) part wakes the worker when program freezes more than 1 sec
        if (_worker.WaitingCallbacks <= 1 && (_worker.CurrentWorkItemsCount == 0 || _worker.InUseThreads == 0))
        {
            _worker.QueueWorkItem(_updateAction);
        }
        else if (_worker.IsIdle || _worker.ActiveThreads == 0)
        {
            _worker.Start();
        }
    }

    public void Dispose(int timeout)
    {
        _worker.Shutdown(timeout);
        _worker.Dispose();
    }
}