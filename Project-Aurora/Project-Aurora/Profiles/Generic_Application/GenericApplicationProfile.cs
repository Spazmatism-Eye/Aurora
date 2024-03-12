using System.Collections.ObjectModel;
using AuroraRgb.Settings;
using AuroraRgb.Settings.Layers;

namespace AuroraRgb.Profiles.Generic_Application
{
    public class GenericApplicationProfile : ApplicationProfile
    {
        

        [Newtonsoft.Json.JsonIgnore]
        public bool _simulateNighttime = false;

        [Newtonsoft.Json.JsonIgnore]
        public bool _simulateDaytime = false;

        public ObservableCollection<Layer> Layers_NightTime { get; set; }

        public GenericApplicationProfile() : base()
        {

        }

        public override void Reset()
        {
            base.Reset();
            Layers_NightTime = new ObservableCollection<Layer>();

        }
    }
}
