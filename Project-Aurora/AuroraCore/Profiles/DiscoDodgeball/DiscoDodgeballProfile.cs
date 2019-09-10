﻿using Aurora.Devices.Layout.Layouts;
using Aurora.Settings;
using Aurora.Settings.Layers;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace Aurora.Profiles.DiscoDodgeball
{
    public class DiscoDodgeballProfile : ApplicationProfile
    {
        public DiscoDodgeballProfile() : base()
        {

        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (!Layers.Any(lyr => lyr.Handler.GetType().Equals(typeof(Aurora.Settings.Layers.WrapperLightsLayerHandler))))
                Layers.Add(new Layer("Wrapper Lighting", new Aurora.Settings.Layers.WrapperLightsLayerHandler()));
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
                        _PrimaryColor = Color.White,
                        _Sequence = new KeySequence(new KeyboardKeys[] { KeyboardKeys.W, KeyboardKeys.A, KeyboardKeys.S, KeyboardKeys.D, KeyboardKeys.SPACE })
                    }
                }),
                new Layer("Wrapper Lighting", new Aurora.Settings.Layers.WrapperLightsLayerHandler()),
            };
        }
    }
}