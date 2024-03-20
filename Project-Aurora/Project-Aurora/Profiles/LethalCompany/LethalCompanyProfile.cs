using System.Drawing;
using Aurora.Settings.Layers;
using Aurora.Settings;
using Aurora.Settings.Overrides.Logic;
using DK = Common.Devices.DeviceKeys;
using Aurora.Profiles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Aurora.Profiles.LethalCompany.GSI.Nodes;

namespace Aurora.Profiles.LethalCompany;

public class LethalCompanyProfile : ApplicationProfile
{
    public override void Reset()
    {
        base.Reset();

        Layers =
        [
            new Layer("Health", new PercentLayerHandler
            {
                Properties = new PercentLayerHandlerProperties
                {
                    VariablePath = new VariablePath("Player/Health"),
                    PercentType = PercentEffectType.Progressive_Gradual,
                    MaxVariablePath = new VariablePath("100"),
                    _PrimaryColor = Color.Lime,
                    _SecondaryColor = Color.FromArgb(20, 0, 0),
                    _Sequence = new KeySequence(new[]
                    {
                        DK.ESC, DK.F1, DK.F2, DK.F3, DK.F4, DK.F5, DK.F6, DK.F7, DK.F8, DK.F9, DK.F10, DK.F11, DK.F12
                    }),
                },
            },
            new OverrideLogicBuilder().SetDynamicBoolean(nameof(PercentLayerHandlerProperties._Enabled), new BooleanGSIEnum("GameState/GameState", GameStateEnum.InGame))
            ),
            new Layer("Stamina", new PercentLayerHandler
            {
                Properties = new PercentLayerHandlerProperties
                {
                    VariablePath = new VariablePath("Player/Stamina"),
                    PercentType = PercentEffectType.Progressive_Gradual,
                    MaxVariablePath = new VariablePath("100"),
                    _PrimaryColor = Color.Orange,
                    _SecondaryColor = Color.FromArgb(20, 0, 0),
                    _Sequence = new KeySequence(new[]
                    {
                        DK.TILDE, DK.ONE, DK.TWO, DK.THREE, DK.FOUR, DK.FIVE, DK.SIX, DK.SEVEN, DK.EIGHT, DK.NINE, DK.ZERO, DK.MINUS, DK.EQUALS, DK.BACKSPACE
                    }),
                    BlinkThreshold = 0,

                }
            },
            new OverrideLogicBuilder().SetDynamicBoolean(nameof(PercentLayerHandlerProperties._Enabled), new BooleanGSIEnum("GameState/GameState", GameStateEnum.InGame))
            ),
            new Layer("Critical Health", new BreathingLayerHandler
            {
                Properties = new BreathingLayerHandlerProperties
                {
                    _EffectSpeed = 5,
                    _SecondaryColor = Color.FromArgb(20, 0, 0),
                    _PrimaryColor = Color.FromArgb(60, 0, 0),
                    _Sequence = new KeySequence(new FreeFormObject(-112, -75, 1116, 345)),
                }
            },
            new OverrideLogicBuilder().SetDynamicFloat(nameof(BreathingLayerHandlerProperties._EffectSpeed), new NumberMap(new NumberGSINumeric("Player/Health"), 0, 19, 15, 5)).SetDynamicBoolean(nameof(BreathingLayerHandlerProperties._Enabled), new BooleanAnd(new List<Evaluatable<bool>> { new BooleanGSIEnum("GameState/GameState", GameStateEnum.InGame), new BooleanMathsComparison(new NumberGSINumeric("Player/Health"), ComparisonOperator.LT, 20) }))
            ),
            new Layer("Background", new SolidFillLayerHandler
            {
                Properties = new SolidFillLayerHandlerProperties
                {
                    _PrimaryColor = Color.FromArgb(20,0,0),
                }
            })
        ];
    }
}