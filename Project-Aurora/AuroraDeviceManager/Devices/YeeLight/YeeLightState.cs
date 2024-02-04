using Common;
using Constants = YeeLightAPI.YeeLightConstants.Constants;

namespace AuroraDeviceManager.Devices.YeeLight;

public interface IYeeLightState
{
    void InitState();
        
    IYeeLightState Update(SimpleColor color);
}

static class YeeLightStateBuilder
{
    public static IYeeLightState Build(List<YeeLightAPI.YeeLightDevice> devices, int whiteCounter)
    {
        var colorState = new YeeLightStateColor(devices, whiteCounter);
        var whiteState = new YeeLightStateWhite(devices);
        var offState = new YeeLightStateOff(devices);
            
        colorState.WhiteState = whiteState;
        colorState.OffState = offState;
        whiteState.ColorState = colorState;
        whiteState.OffState = offState;
        offState.ColorState = colorState;

        return offState;
    }
}

internal class YeeLightStateOff(List<YeeLightAPI.YeeLightDevice> lights) : IYeeLightState
{
    public IYeeLightState ColorState;

    public void InitState()
    {
        TurnOff();
    }

    public IYeeLightState Update(SimpleColor color)
    {
        if (Utils.IsBlack(color))
        {
            return this;
        }

        TurnOn();
        ColorState.InitState();
        var newState = ColorState.Update(color);
        return newState;
    }
        
    private void TurnOff()
    {
        lights.ForEach(device => device.SetPower(Constants.PowerStateParamValues.OFF));
    }
        
    private void TurnOn()
    {
        lights.ForEach(device => device.SetPower(Constants.PowerStateParamValues.ON));
    }
}
    
internal class YeeLightStateColor(List<YeeLightAPI.YeeLightDevice> lights, int whiteCounterStart) : IYeeLightState
{
    public IYeeLightState WhiteState;
    public IYeeLightState OffState;

    private int _previousBrightness;
    private SimpleColor _previousColor;
    private int _whiteCounter;

    public void InitState()
    {
        //noop
    }

    public IYeeLightState Update(SimpleColor color)
    {
        if (Utils.IsWhiteTone(color)) // && Global.LightingStateManager.GetCurrentProfile() == Global.LightingStateManager.DesktopProfile
        {
            if (Utils.IsBlack(color))
            {
                OffState.InitState();
                return OffState.Update(color);
            }
            if (_whiteCounter-- <= 0)
            {
                WhiteState.InitState();
                return WhiteState.Update(color);
            }
        }
        else
        {
            _whiteCounter = whiteCounterStart;
        }

        if (Utils.ShouldSendKeepAlive())
        {
            lights.ForEach(device => device.SetPower(Constants.PowerStateParamValues.ON));
        }
            
        if (_previousColor == color)
            return ProceedSameColor(color);
        _previousColor = color;
            
        //color changed
        UpdateLights(color);
        return this;
    }

    private IYeeLightState ProceedSameColor(SimpleColor targetColor)
    {
        if (!Utils.ShouldSendKeepAlive()) return this;
        lights.ForEach(device => device.SetPower(Constants.PowerStateParamValues.ON));
        UpdateLights(targetColor);
        return this;
    }

    private readonly List<Task> _tasks = [];
    private void UpdateLights(SimpleColor color)
    {
        lights.ForEach(x =>
        {
            var colorAsync = x.SetColorAsync(color.R, color.G, color.B);
            _tasks.Add(colorAsync);
            var brightness = Math.Max(color.R, Math.Max(color.G, Math.Max(color.B, (short) 1))) * 100 / 255;
            if (_previousBrightness == brightness) return;
            _previousBrightness = brightness;
            var brightnessAsync = x.SetBrightnessAsync(brightness);
            _tasks.Add(brightnessAsync);
        });
        Task.WhenAll(_tasks).Wait();
        _tasks.Clear();
    }
}
    
internal class YeeLightStateWhite : IYeeLightState
{
    public IYeeLightState ColorState;
    public IYeeLightState OffState;

    private readonly List<YeeLightAPI.YeeLightDevice> _lights;

    private SimpleColor _previousColor;

    public YeeLightStateWhite(List<YeeLightAPI.YeeLightDevice> lights)
    {
        _lights = lights;
    }

    public void InitState()
    {
        var tasks = _lights.ConvertAll(x => x.SetColorTemperatureAsync(6500));
        Task.WhenAll(tasks).Wait();
    }

    public IYeeLightState Update(SimpleColor color)
    {
        if (!Utils.IsWhiteTone(color))
        {
            ColorState.InitState();
            return ColorState.Update(color);
        }
            
        if (Utils.IsBlack(color))
        {
            OffState.InitState();
            return OffState.Update(color);
        }
            
        if (_previousColor == color)
        {
            if (Utils.ShouldSendKeepAlive())
            {
                InitState();
                UpdateLights(color);
            }
            return this;
        }

        _previousColor = color;
            
        //color changed
        UpdateLights(color);
        return this;
    }

    private void UpdateLights(SimpleColor color)
    {
        var tasks = _lights.ConvertAll(x => x.SetBrightnessAsync(color.R * 100 / 255));
        Task.WhenAll(tasks).Wait();
    }
}

internal static class Utils
{
    internal static bool IsWhiteTone(SimpleColor color)
    {
        return color.R == color.G && color.G == color.B;
    }
    internal static bool IsBlack(SimpleColor color)
    {
        return color is { R: 0, G: 0, B: 0 };
    }

    private const int KeepAliveCounter = 500;
    private static int _keepAlive = KeepAliveCounter;
    internal static bool ShouldSendKeepAlive()
    {
        if (_keepAlive-- != 0) return false;
        _keepAlive = KeepAliveCounter;
        return true;
    }
}