using Common.Data;
using Microsoft.Scripting.Utils;

namespace Common.Utils;

internal static class MemorySharedEventThread
{
    private static readonly List<HandlesAndThread> HandleThreads = [];

    internal static void AddObject(SignaledMemoryObject o)
    {
        var handleThread = HandleThreads.Find(ht => ht.HasSpace(2));
        if (handleThread == null)
        {
            handleThread = new HandlesAndThread();
            HandleThreads.Add(handleThread);
        }
        
        handleThread.AddToThread(o);
    }

    internal static void RemoveObject(SignaledMemoryObject o)
    {
        foreach (var handlesAndThread in HandleThreads)
        {
            handlesAndThread.RemoveIfExists(o);
        }
    }

    private sealed class HandlesAndThread
    {
        private const int MaxHandles = 64;
        
        private readonly SemaphoreSlim _semaphore = new(1);
    
        private CancellationTokenSource _cancellation = new();
        private CancellationTokenSource CancelToken
        {
            get => _cancellation;
            set
            {
                var old = _cancellation;
                _cancellation = value;
                _handles[0] = value.Token.WaitHandle;
                old.Cancel();
                _threadCreated.Wait();
                old.Dispose();
            }
        }
        
        private Thread _thread = new(() => { });
        
        private Action[] _actions = [() => { }];
        private WaitHandle[] _handles;
        private Task _threadCreated = Task.CompletedTask;

        internal HandlesAndThread()
        {
            _handles = [CancelToken.Token.WaitHandle];
            _thread.Start();
        }

        private Thread CreateThread()
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            _threadCreated = tcs.Task;
            var thread = new Thread(() =>
            {
                tcs.SetResult();
                if (_handles.Length <= 1)
                {
                    // stop thread if only handle is cancel token
                    return;
                }
                while (true)
                {
                    var i = WaitHandle.WaitAny(_handles);
                    switch (i)
                    {
                        case 0:
                            return;
                        default:
                            Task.Run(() =>
                            {
                                _actions[i].Invoke();
                            });
                            break;
                    }
                }
            })
            {
                Name = "Memory Share Event Thread",
                IsBackground = true,
                Priority = ThreadPriority.Highest,
            };
            thread.Start();
            _threadCreated.Wait(200);
            Thread.Yield();
            Thread.Sleep(1); //wait until WaitHandle.WaitAny(_handles) runs before returning
            return thread;
        }

        internal void AddToThread(SignaledMemoryObject o)
        {
            _semaphore.Wait();

            try
            {
                _actions = _actions.Concat([
                    o.OnUpdated,
                    o.OnUpdateRequested
                ]).ToArray();
                _handles = _handles.Concat([
                    o.ObjectUpdatedHandle,
                    o.UpdateRequestedHandle
                ]).ToArray();

                CancelToken = new CancellationTokenSource();
                _thread = CreateThread();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        internal void RemoveIfExists(SignaledMemoryObject o)
        {
            _semaphore.Wait();

            try
            {
                var updatedHandleIndex = _handles.FindIndex(h => o.ObjectUpdatedHandle == h);
                var requestedHandleIndex = _handles.FindIndex(h => o.UpdateRequestedHandle == h);

                if (updatedHandleIndex == -1)
                {
                    return;
                }
                
                _actions = _actions.Where((_, i) => i != updatedHandleIndex && i != requestedHandleIndex).ToArray();
                _handles = _handles.Where((_, i) => i != updatedHandleIndex && i != requestedHandleIndex).ToArray();

                CancelToken = new CancellationTokenSource();
                _thread = CreateThread();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        internal bool HasSpace(int handleCount)
        {
            return _handles.Length + handleCount < MaxHandles;
        }
    }
}