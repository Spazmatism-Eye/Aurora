using AuroraRgb.Profiles;
using Common.Devices;

namespace AuroraRgb.Settings;

public interface IEffectScript
{
    string? ID { get; }

    VariableRegistry Properties { get; }

    object UpdateLights(VariableRegistry properties, IGameState state = null);
}