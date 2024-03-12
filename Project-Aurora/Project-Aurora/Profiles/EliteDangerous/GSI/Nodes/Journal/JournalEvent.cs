using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuroraRgb.Profiles.EliteDangerous.Journal
{
    public enum EventType
    {
        FSDTarget,
        StartJump,
        SupercruiseEntry,
        SupercruiseExit,
        Fileheader,
        FSDJump,
        Loadout,
        Music,
        LaunchFighter,
        DockFighter,
        FighterDestroyed,
        FighterRebuilt,
    }

    public class JournalEvent
    {
        public DateTime timestamp;
        public EventType @event;
    }
    
    public class JournalEventJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(JournalEvent).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, 
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            switch (item["event"].Value<string>())
            {
                case "FSDTarget": return item.ToObject<Events.FSDTarget>();
                case "StartJump": return item.ToObject<Events.StartJump>();
                case "SupercruiseEntry": return item.ToObject<Events.SupercruiseEntry>();
                case "SupercruiseExit": return item.ToObject<Events.SupercruiseExit>();
                case "Fileheader": return item.ToObject<Events.Fileheader>();
                case "FSDJump": return item.ToObject<Events.FSDJump>();
                case "Loadout": return item.ToObject<Events.Loadout>();
                case "Music": return item.ToObject<Events.Music>();
                case "LaunchFighter": return item.ToObject<Events.LaunchFighter>();
                case "DockFighter": return item.ToObject<Events.DockFighter>();
                case "FighterDestroyed": return item.ToObject<Events.FighterDestroyed>();
                case "FighterRebuilt": return item.ToObject<Events.FighterRebuilt>();
            }
                
            //Do not deserialize an event we don't need since it's REALLY SLOW!
            return null;
        }

        public override void WriteJson(JsonWriter writer, 
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}