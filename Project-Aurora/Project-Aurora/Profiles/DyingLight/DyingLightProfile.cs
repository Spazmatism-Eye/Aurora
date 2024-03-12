using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;
using Common.Devices;

namespace AuroraRgb.Profiles.DyingLight
{
    public class DyingLightProfile : ApplicationProfile
    {
        public DyingLightProfile() : base()
        {
            
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (!Layers.Any(lyr => lyr.Handler.GetType().Equals(typeof(WrapperLightsLayerHandler))))
                Layers.Add(new Layer("Wrapper Lighting", new WrapperLightsLayerHandler()));
        }


        public override void Reset()
        {
            base.Reset();
            Layers = new System.Collections.ObjectModel.ObservableCollection<Layer>()
            {
                new Layer("Movement", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.DodgerBlue,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.W, DeviceKeys.A, DeviceKeys.S, DeviceKeys.D, DeviceKeys.SPACE, DeviceKeys.C, DeviceKeys.LEFT_SHIFT })
                    }
                }
                ),
                new Layer("Actions", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.DarkOrange,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.Q, DeviceKeys.E, DeviceKeys.R, DeviceKeys.F, DeviceKeys.V, DeviceKeys.B })
                    }
                }
                ),
                new Layer("Inventory", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Magenta,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.I, DeviceKeys.M, DeviceKeys.L, DeviceKeys.U})
                    }
                }
                ),
                new Layer("Healing", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Green,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.H}),
                    }
                }
                ),
                new Layer("Flashlight", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.White,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.T})
                    }
                }),
                new Layer("Wrapper Lighting", new WrapperLightsLayerHandler()
                {
                    Properties = new WrapperLightsLayerHandlerProperties()
                    {
                        ColorEnhanceMode = 1            
                    }
                })
            };
        }
    }
}
