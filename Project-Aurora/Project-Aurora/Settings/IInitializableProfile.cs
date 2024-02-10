using System;

namespace Aurora.Settings;

public interface IInitializableProfile : IDisposable
{
    bool Initialized { get; }

    bool Initialize();
}