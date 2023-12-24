using System;
using System.Windows.Controls;
using Aurora.EffectsEngine;
using Aurora.Profiles;
using Newtonsoft.Json;
using System.ComponentModel;
using Aurora.Settings.Layers.Controls;
using Aurora.Settings.Overrides;
using Common.Devices;

namespace Aurora.Settings.Layers;

public class ScriptLayerHandlerProperties : LayerHandlerProperties<ScriptLayerHandlerProperties>
{
    public string? _Script { get; set; } = null;

    [JsonIgnore]
    public string Script => Logic._Script ?? _Script ?? String.Empty;

    private VariableRegistry? _scriptProperties;

    [JsonProperty("_ScriptProperties")]
    public VariableRegistry ScriptProperties
    {
        get => Logic._scriptProperties ?? _scriptProperties ?? throw new NullReferenceException("ScriptLayerHandlerProperties._scriptProperties is null");
        set => _scriptProperties = value;
    }

    public ScriptLayerHandlerProperties() { }

    public ScriptLayerHandlerProperties(bool empty = false) : base(empty) { }

    public override void Default()
    {
        base.Default();
        _scriptProperties = new VariableRegistry();
    }
}

[LogicOverrideIgnoreProperty("_PrimaryColor")]
[LogicOverrideIgnoreProperty("_Sequence")]
public class ScriptLayerHandler : LayerHandler<ScriptLayerHandlerProperties>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    [JsonIgnore]
    public Exception ScriptException { get; private set; }

    public override EffectLayer Render(IGameState gamestate)
    {

        if (!IsScriptValid) return EffectLayer.EmptyLayer;
        try
        {
            var script = Application.EffectScripts[Properties.Script];
            var scriptLayers = script.UpdateLights(Properties.ScriptProperties, gamestate);
            EffectLayer.Clear();
            switch (scriptLayers)
            {
                case EffectLayer layers:
                    EffectLayer.Add(layers);
                    break;
                case EffectLayer[] effectLayers:
                {
                    for (var i = 1; i < effectLayers.Length; i++)
                        EffectLayer.Add(effectLayers[i]);
                    break;
                }
            }
            ScriptException = null;
        }
        catch(Exception exc)
        {
            Global.logger.Error(exc, "Effect script with key {PropertiesScript} encountered an error", Properties.Script);
            ScriptException = exc;
        }

        return EffectLayer;
    }

    public VariableRegistry? GetScriptPropertyRegistry()
    {
        if (IsScriptValid)
        {
            return Application.EffectScripts[Properties._Script].Properties;
        }

        return null;
    }

    public void OnScriptChanged()
    {
        if (Properties._Script == null || !Application.EffectScripts.ContainsKey(Properties._Script))
        {
            return;
        }
        Properties.ScriptProperties = Application.EffectScripts[Properties._Script].Properties;
    }

    [JsonIgnore]
    public bool IsScriptValid => Application?.EffectScripts?.ContainsKey(Properties.Script) ?? false;

    public override void SetApplication(Application profile)
    {
        Application = profile;
        base.SetApplication(profile);
        OnScriptChanged();
    }

    protected override UserControl CreateControl()
    {
        return new Control_ScriptLayer(this);
    }
}