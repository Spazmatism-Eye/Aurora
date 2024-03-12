using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;
using Common.Devices;

namespace AuroraRgb.Profiles.QuantumConumdrum
{
    public class QuantumConumdrumProfile : ApplicationProfile
    {
        public QuantumConumdrumProfile() : base()
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
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.W, DeviceKeys.A, DeviceKeys.S, DeviceKeys.D, DeviceKeys.SPACE})
                    }
                }
                ),
                new Layer("Fluffy", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Pink,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.Q})
                    }
                }
                ),
                new Layer("Heavy", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Red,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.E})
                    }
                }
                ),
                new Layer("Slow", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Yellow,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.ONE})
                    }
                }
                ),
                new Layer("Reverse", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Green,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.THREE})
                    }
                }
                ),
                new Layer("Wrapper Lighting", new WrapperLightsLayerHandler())
            };
        }
    }
}
