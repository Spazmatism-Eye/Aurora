using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aurora.Settings.Layers;
using Aurora.Settings.Overrides.Logic;
using Common.Devices;
using Newtonsoft.Json.Linq;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Aurora.Utils;

public class AuroraSerializationBinder : DefaultSerializationBinder
{
    private readonly Dictionary<string, Dictionary<string, Type>> _assemblyTypeMap = new();
    private readonly Dictionary<string, Type> _typeMap = new();
    
    public override Type BindToType(string? assemblyName, string typeName)
    {
        const string pattern1 = @"\[([^\s,^\[]*), ([^\s,^\]]*)]";

        Dictionary<string, Type> typeMap;
        if (assemblyName != null)
        {
            if (!_assemblyTypeMap.TryGetValue(assemblyName, out typeMap!))
            {
                typeMap = new Dictionary<string, Type>();
                _assemblyTypeMap[assemblyName] = typeMap;
            }
        }
        else
        {
            typeMap = _typeMap;
        }
        if (typeMap.TryGetValue(typeName, out var type))
        {
            return type;
        }

        // Use the Regex.Replace method with a MatchEvaluator delegate
        var convertedTypeName = Regex.Replace(typeName, pattern1, ReplaceGroups);

        var boundType = convertedTypeName switch
        {
            "System.Windows.Forms.Keys" =>
                typeof(System.Windows.Forms.Keys),
            "System.Collections.Generic.Queue`1[[System.Windows.Forms.Keys, System.Windows.Forms]]" =>
                typeof(List<System.Windows.Forms.Keys>),
            //Resolve typo'd AbilityLayerHandler type
            "Aurora.Profiles.Dota_2.Layers.Dota2AbiltiyLayerHandler" =>
                typeof(Profiles.Dota_2.Layers.Dota2AbilityLayerHandler),
            "Aurora.Profiles.Dota_2.Layers.Dota2HeroAbiltiyEffectsLayerHandler" =>
                typeof(Profiles.Dota_2.Layers.Dota2HeroAbilityEffectsLayerHandler),
            "Aurora.Settings.Layers.ComparisonLayerHandler" =>
                typeof(DefaultLayerHandler),
            "Aurora.Settings.Layers.ComparisonLayerProperties" =>
                typeof(LayerHandlerProperties),
            "Aurora.Profiles.Dota_2.Layers.Dota2HeroAbiltiyEffectsLayerHandlerProperties" =>
                typeof(Profiles.Dota_2.Layers.Dota2HeroAbilityEffectsLayerHandlerProperties),
            "Aurora.Profiles.TheDivision.TheDivisionSettings" =>
                typeof(Settings.ApplicationProfile),
            "Aurora.Profiles.Overwatch.OverwatchProfile" =>
                typeof(Profiles.WrapperProfile),
            "Aurora.Profiles.WormsWMD.WormsWMDProfile" =>
                typeof(Profiles.WrapperProfile),
            "Aurora.Profiles.Blade_and_Soul.BnSProfile" =>
                typeof(Profiles.WrapperProfile),
            "Aurora.Profiles.Magic_Duels_2012.MagicDuels2012Profile" =>
                typeof(Profiles.WrapperProfile),
            "Aurora.Profiles.ColorEnhanceProfile" =>
                typeof(Profiles.WrapperProfile),
            "Aurora.Settings.Overrides.Logic.IEvaluatableBoolean" =>
                typeof(Evaluatable<bool>),
            "Aurora.Settings.Overrides.Logic.IEvaluatable`1[[System.Boolean, mscorlib]]" =>
                typeof(Evaluatable<bool>),
            "Aurora.Settings.Overrides.Logic.IEvaluatableNumber" =>
                typeof(Evaluatable<double>),
            "Aurora.Settings.Overrides.Logic.IEvaluatable`1[[System.Double, mscorlib]]" =>
                typeof(Evaluatable<double>),
            "Aurora.Settings.Overrides.Logic.IEvaluatableString" =>
                typeof(Evaluatable<string>),
            "Aurora.Settings.Overrides.Logic.IEvaluatable`1[[System.String, mscorlib]]" =>
                typeof(Evaluatable<string>),
            "Aurora.Settings.Overrides.Logic.Boolean.Boolean_Latch" =>
                typeof(Settings.Overrides.Logic.Boolean.Boolean_FlipFlopSR),
            "System.Drawing.Color" =>
                typeof(Color),
            "Aurora.Devices.DeviceKeys" =>
                typeof(DeviceKeys),
            "Common.Devices.DeviceKeys" =>
                typeof(DeviceKeys),
            "Aurora.Settings.VariableRegistry" =>
                typeof(VariableRegistry),
            "Aurora.Settings.VariableRegistryItem" =>
                typeof(VariableRegistryItem),
            _ => base.BindToType(assemblyName, convertedTypeName),
        };
        typeMap[typeName] = boundType;

        return boundType;
    }
    
    private string ReplaceGroups(Match match)
    {
        var typeName = match.Groups[1].Value;
        var assemblyName = match.Groups[2].Value;

        var type = BindToType(assemblyName, typeName);
        var ass = type.AssemblyQualifiedName;
        var secondComma = ass.IndexOf(',', ass.IndexOf(',') + 1);
        return string.Concat("[", ass.AsSpan(0, secondComma), "]");
    }
}

public class EnumConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType.IsEnum;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteStartObject();
        writer.WritePropertyName("$type");
        writer.WriteValue(value.GetType().AssemblyQualifiedName);
        writer.WritePropertyName("$value");
        serializer.Serialize(writer, value);
        writer.WriteEndObject();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // If this is a null token, then the original value was null.
        var readerTokenType = reader.TokenType;
        switch (readerTokenType)
        {
            case JsonToken.Null:
                return null;
            case JsonToken.Integer:
                return Enum.ToObject(objectType, reader.Value);
            case JsonToken.StartObject:
                var item = serializer.Deserialize<JObject>(reader);
                var typeName = item["$type"].Value<string>();
                return JsonConvert.DeserializeObject(item["$value"].ToString(), Type.GetType(typeName));
        }

        return existingValue;
    }
}

public class OverrideTypeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType.FullName == typeof(Type).FullName;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteStartObject();
        writer.WritePropertyName("$type");
        writer.WriteValue(value.GetType().AssemblyQualifiedName);
        writer.WritePropertyName("$value");
        serializer.Serialize(writer, value);
        writer.WriteEndObject();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // If this is a null token, then the original value was null.
        var readerTokenType = reader.TokenType;
        switch (readerTokenType)
        {
            case JsonToken.String:
                return Type.GetType(reader.Value.ToString());
            case JsonToken.StartObject:
                var item = serializer.Deserialize<JObject>(reader);
                foreach (var prop in item.Children<JProperty>())
                {
                    switch (prop.Name)
                    {
                        case "$type":
                            return Type.GetType(prop.Value.ToString());
                    }
                }

                break;
            case JsonToken.Null:
                return objectType;
            default:
                return objectType;
        }

        return existingValue;
    }
}

public class TypeAnnotatedObjectConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType.FullName == typeof(Color).FullName ||
                                                        objectType.IsAssignableFrom(typeof(object));

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteStartObject();
        writer.WritePropertyName("$type");
        writer.WriteValue(value.GetType().AssemblyQualifiedName);
        writer.WritePropertyName("$value");
        serializer.Serialize(writer, value);
        writer.WriteEndObject();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // If this is a null token, then the original value was null.
        var readerTokenType = reader.TokenType;
        if (readerTokenType == JsonToken.StartObject)
        {
            return ParseJsonNode(reader, objectType, serializer);
        }
        
        var json = reader.Value?.ToString();
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }
        switch (readerTokenType)
        {
            case JsonToken.String:
                return json.StartsWith('\"')
                    ? JsonConvert.DeserializeObject(json, objectType)
                    : JsonConvert.DeserializeObject("\"" + json + "\"", objectType);
            case JsonToken.Integer:
                return JsonConvert.DeserializeObject(json, objectType);
            case JsonToken.Boolean:
                switch (json.ToLowerInvariant())
                {
                    case "true":
                        return true;
                    case "false":
                        return false;
                    default:
                        return false;
                }
            case JsonToken.Null:
                return existingValue;
        }

        Global.logger.Warning("Unhandled json type {Token}", readerTokenType);
        return JsonConvert.DeserializeObject(json, objectType);
    }

    private static object? ParseJsonNode(JsonReader reader, Type objectType, JsonSerializer serializer)
    {
        var item = serializer.Deserialize<JObject>(reader);
        var jToken = item?["$type"];
        if (item == null || jToken == null)
        {
            throw new JsonReaderException("item or $type is null");
        }
        var type = serializer.Deserialize<Type>(jToken.CreateReader());
        var value = item["$value"];
        if (value == null)
        {
            return serializer.Deserialize(item.CreateReader(), type);
        }

        var valueReader = value.CreateReader();
        switch (valueReader.TokenType)
        {
            case JsonToken.StartObject:
                return serializer.Deserialize(valueReader, type);
            default:
                var s = value.ToString();
                if (type == typeof(bool) || type == typeof(Color))
                {
                    return JsonConvert.DeserializeObject("\"" + value + "\"", type);
                }
                if (objectType.FullName != typeof(Color).FullName && type?.FullName != typeof(Color).FullName)
                    return JsonConvert.DeserializeObject(s, type);
                if (s.StartsWith('\"'))
                {
                    return JsonConvert.DeserializeObject(s, type);
                }

                Global.logger.Error("Attempting to convert unknown type: {Type}", type);
                return JsonConvert.DeserializeObject("\"" + value + "\"", type);
        }
    }
}

public class SingleToDoubleConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(double);

    public override bool CanRead => true;

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Float:
            case JsonToken.Integer:
                return (double)(reader.Value ?? 0.0);
            case JsonToken.String:

                double.TryParse(reader.ReadAsString(), out var value);
                return value;
        }

        throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
    }
}

public class DateTimeOffsetConverterUsingDateTimeParse : System.Text.Json.Serialization.JsonConverter<DateTimeOffset >
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.Parse(reader.GetString() ?? "");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset  value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class VariableRegistryDictionaryConverter : JsonConverter<IDictionary<string, VariableRegistryItem>>
{
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, IDictionary<string, VariableRegistryItem>? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override IDictionary<string, VariableRegistryItem>? ReadJson(JsonReader reader, Type objectType, IDictionary<string, VariableRegistryItem>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var readerTokenType = reader.TokenType;
        switch (readerTokenType)
        {
            case JsonToken.StartObject:
                var json = serializer.Deserialize<JObject>(reader);
                if (json == null)
                {
                    return existingValue;
                }

                var value = json["$values"];
                var valueReader = value?.CreateReader();
                if (valueReader != null)
                    return ParsePairArray(existingValue, hasExistingValue, serializer, valueReader);
                
                var map = existingValue ?? new Dictionary<string, VariableRegistryItem>();
                foreach (var prop in json.Children<JProperty>())
                {
                    if (prop.Name.Equals("$type"))
                    {
                        continue;
                    }

                    var key = prop.Name;
                    var item = serializer.Deserialize<VariableRegistryItem>(prop.Value.CreateReader())!;
                    map.TryAdd(key, item);
                }

                return map;

            case JsonToken.StartArray:
                return serializer.Deserialize(reader, typeof(IDictionary<dynamic, dynamic>)) as IDictionary<string, VariableRegistryItem>;
        }

        throw new JsonSerializationException("Unexpected json state");
    }

    private static IDictionary<string, VariableRegistryItem>? ParsePairArray(IDictionary<string, VariableRegistryItem>? existingValue, bool hasExistingValue, JsonSerializer serializer,
        JsonReader valueReader)
    {
        var list = serializer.Deserialize(valueReader, typeof(List<(string, VariableRegistryItem)>)) as List<(string, VariableRegistryItem)>;
        if (!hasExistingValue || existingValue == null) 
            return list?.ToDictionary(e => e.Item1, e => e.Item2);

        if (list == null)
        {
            return existingValue;
        }
        foreach (var valueTuple in list)
        {
            existingValue[valueTuple.Item1] = valueTuple.Item2;
        }
        return existingValue;
    }
}
