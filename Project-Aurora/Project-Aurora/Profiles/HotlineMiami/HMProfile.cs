﻿using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;
using Common.Devices;

namespace AuroraRgb.Profiles.HotlineMiami
{
    public class HMProfile : ApplicationProfile
    {
        public HMProfile() : base()
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
                        _PrimaryColor = Color.Yellow,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.W, DeviceKeys.A, DeviceKeys.S, DeviceKeys.D })
                    }
                }
                ),
                new Layer("Other Actions", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Red,
                        _Sequence = new KeySequence(new DeviceKeys[] { DeviceKeys.SPACE, DeviceKeys.LEFT_SHIFT, DeviceKeys.R, DeviceKeys.ESC })
                    }
                }),
                new Layer("Wrapper Lighting", new WrapperLightsLayerHandler()),
            };
        }
    }
}
