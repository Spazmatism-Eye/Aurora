using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aurora.Utils;

public class UltimateListJsonConverter : JsonConverter<IList<dynamic>>
{
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, IList<dynamic>? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override IList<dynamic>? ReadJson(JsonReader reader, Type objectType, IList<dynamic>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var genericTypes = objectType.GenericTypeArguments;
        var itemType = objectType.HasElementType ? objectType.GetElementType()! : genericTypes[0];

        var list = existingValue ?? (objectType.IsAbstract ? new List<dynamic>() : ListInstance(objectType));

        var jsonType = reader.TokenType;
        switch (jsonType)
        {
            case JsonToken.StartObject: //this is $type annotated
                var rootObject = serializer.Deserialize<JObject>(reader);
                if (rootObject == null)
                {
                    return null;
                }

                var values = rootObject["$values"];
                if (values == null) 
                    throw new NotImplementedException("Unknown list json object: " + rootObject);

                if (objectType.IsArray)
                {
                    return ParseArray(values, itemType, serializer);
                }
                ParseItems(values, list, itemType, serializer);
                return list;

            case JsonToken.StartArray:
                var rootArray = serializer.Deserialize<JArray>(reader)!;
                if (objectType.IsArray)
                {
                    return ParseArray(rootArray, itemType, serializer);
                }
                ParseItems(rootArray, list, itemType, serializer);
                return list;
        }

        throw new NotImplementedException("Unknown list json type:" + jsonType);
    }

    private void ParseItems(JToken values, IList<dynamic> list, Type itemType, JsonSerializer serializer)
    {
        foreach (var itemObject in values.Children<JObject>())
        {
            var item = ParseItem(itemType, serializer, itemObject);
            list.Add(item);
        }
    }

    private dynamic[] ParseArray(JToken values, Type itemType, JsonSerializer serializer)
    {
        var objects = values.Children<JObject>()
            .Select(itemObject => ParseItem(itemType, serializer, itemObject))
            .ToArray();
        
        return (dynamic[])CastArray(objects, itemType);
    }
    
    private static Array CastArray(Array inputArray, Type targetType)
    {
        // Check if the input array type is compatible with the target type
        if (inputArray.GetType().GetElementType() == targetType)
        {
            return inputArray; // No need to cast if they are already of the same type
        }

        // Create a new array of the target type and copy elements
        var newArray = Array.CreateInstance(targetType, inputArray.Length);
        Array.Copy(inputArray, newArray, inputArray.Length);
        return newArray;
    }

    private static dynamic ParseItem(Type itemType, JsonSerializer serializer, JObject itemObject)
    {
        switch (itemObject.Type)
        {
            case JTokenType.Object:
            case JTokenType.Boolean:
            case JTokenType.Date:
            case JTokenType.Float:
            case JTokenType.Integer:
            case JTokenType.String:
            case JTokenType.TimeSpan:
                return serializer.Deserialize(itemObject.CreateReader(), itemType)!;
            default:
                throw new NotImplementedException("Unexpected array object type: " + itemObject.Type);
        }
    }

    private static IList<dynamic> ListInstance(Type objectType)
    {
        var constructorInfo = objectType.GetConstructor(Type.EmptyTypes);
        if (constructorInfo == null)
        {
            throw new JsonException("Type " + objectType + " does not have parameterless constructor");
        }
        return (IList<dynamic>)Activator.CreateInstance(objectType)!;
    }
}

public class ObservableCollectionJsonConverter : JsonConverter<ObservableCollection<dynamic>>
{
    private readonly UltimateListJsonConverter _listJsonConverter = new();
    
    public override bool CanWrite => _listJsonConverter.CanWrite;

    public override void WriteJson(JsonWriter writer, ObservableCollection<dynamic>? value, JsonSerializer serializer)
    {
        _listJsonConverter.WriteJson(writer, value, serializer);
    }

    public override ObservableCollection<dynamic>? ReadJson(JsonReader reader, Type objectType, ObservableCollection<dynamic>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var list = _listJsonConverter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        return list != null ? new ObservableCollection<dynamic>(list) : null;
    }
}