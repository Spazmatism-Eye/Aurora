﻿using Aurora.Settings;
using Aurora.Settings.Layers;
using System.Collections.Generic;
using System.Drawing;

namespace Aurora.Profiles.Evolve
{
    public class EvolveProfile : ApplicationProfile
    {
        //Effects
        //// Lighting Areas
        public List<ColorZone> lighting_areas { get; set; }

        public EvolveProfile() : base()
        {

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
                        _PrimaryColor = Color.DarkRed,
                        _Sequence = new KeySequence(new Devices.DeviceKeys[] { Devices.DeviceKeys.W, Devices.DeviceKeys.A, Devices.DeviceKeys.S, Devices.DeviceKeys.D, Devices.DeviceKeys.SPACE, Devices.DeviceKeys.LEFT_CONTROL })
                    }
                }
                ),
                new Layer("Other Actions", new SolidColorLayerHandler()
                {
                    Properties = new LayerHandlerProperties()
                    {
                        _PrimaryColor = Color.OrangeRed,
                        _Sequence = new KeySequence(new Devices.DeviceKeys[] { Devices.DeviceKeys.ONE, Devices.DeviceKeys.TWO, Devices.DeviceKeys.THREE, Devices.DeviceKeys.FOUR })
                    }
                }
                )
            };

            //Effects
            //// Lighting Areas
            lighting_areas = new List<ColorZone>();
        }
    }
}