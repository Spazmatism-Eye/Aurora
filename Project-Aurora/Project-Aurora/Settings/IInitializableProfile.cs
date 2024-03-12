using System;

namespace AuroraRgb.Settings;

public interface IInitializableProfile : IDisposable
{
    bool Initialized { get; }

    bool Initialize();
}