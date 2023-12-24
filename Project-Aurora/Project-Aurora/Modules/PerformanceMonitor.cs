﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;
using Aurora.Modules.ProcessMonitor;
using Aurora.Utils;

namespace Aurora.Modules;

public sealed class PerformanceMonitor : AuroraModule
{
    private const string RzChromaStreamStartProcessName = "rzchromastreamserver.exe";
    private const string RzChromaStreamStopProcessName = "rzchromastream";
    private const string RazerChromeServerProcessName = "RzChromaStreamServer";

    private static readonly bool EnableChromaMonitor = false;

    private readonly Task<RunningProcessMonitor> _runningProcessMonitor;

    private readonly SmartThreadPool _threadPool = new(new STPStartInfo
    {
        AreThreadsBackground = true,
        ThreadPriority = ThreadPriority.BelowNormal,
        IdleTimeout = 1000,
    })
    {
        Name = "Performance Monitor Thread",
        Concurrency = 1,
        MaxThreads = 1,
    };

    private readonly TaskCompletionSource _endTrigger = new();
    private bool _working;

    private PerformanceCounter? _auroraMemCounter;
    private PerformanceCounter? _rzStreamCpuCounter;

    public PerformanceMonitor(Task<RunningProcessMonitor> runningProcessMonitor)
    {
        _runningProcessMonitor = runningProcessMonitor;
    }

    protected override async Task Initialize()
    {
        InitializeAurora();
        if (EnableChromaMonitor)
        {
            await InitializeRazerStreamApi();
        }

        _working = true;
        _threadPool.QueueWorkItem((Action)MonitorPerformance);
        return;

        void MonitorPerformance()
        {
            CheckAuroraMemory();
            CheckRazerStreamApi();

            Task.WaitAny(Task.Delay(TimeSpan.FromSeconds(20)), _endTrigger.Task);

            if (_working)
            {
                _threadPool.QueueWorkItem((Action)MonitorPerformance);
            }
        }
    }

    public override async Task DisposeAsync()
    {
        (await _runningProcessMonitor).RunningProcessesChanged -= ProcessMonitorOnRunningProcessesChanged;
        
        _working = false;
        _endTrigger.SetResult();
        _threadPool.Join();
    }

    public override void Dispose()
    {
        DisposeAsync().Wait();
    }

    private void InitializeAurora()
    {
        _auroraMemCounter = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
    }

    private void CheckAuroraMemory()
    {
        var currentRam = _auroraMemCounter?.NextValue() / 1024 / 1024;
        if (!(currentRam > 3500)) return; // around 2,5GB limit
        Global.logger.Fatal("Aurora memory limit exceeded! Closing Aurora to preserve system stability");
        Environment.Exit(10);
    }

    private async Task InitializeRazerStreamApi()
    {
        if ((await _runningProcessMonitor).IsProcessRunning(RzChromaStreamStartProcessName))
        {
            _rzStreamCpuCounter = new PerformanceCounter("Process", "% Processor Time", RazerChromeServerProcessName, true);
        }
        (await _runningProcessMonitor).RunningProcessesChanged += ProcessMonitorOnRunningProcessesChanged;
    }

    private void ProcessMonitorOnRunningProcessesChanged(object? sender, RunningProcessChanged e)
    {
        _rzStreamCpuCounter = e.ProcessName switch
        {
            RzChromaStreamStartProcessName => new PerformanceCounter("Process", "% Processor Time",
                RazerChromeServerProcessName, true),
            RzChromaStreamStopProcessName => null,
            _ => _rzStreamCpuCounter
        };
    }

    private void CheckRazerStreamApi()
    {
        if (_rzStreamCpuCounter == null)
        {
            return;
        }

        try
        {
            var cpuUsage = _rzStreamCpuCounter.NextValue();
            if (cpuUsage > 80)
            {
                RazerChromaUtils.DisableChromaBloat();
            }
        }
        catch { /* ignore */ }
    }
}