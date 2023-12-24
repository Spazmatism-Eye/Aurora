using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Utils;

public static class TypeExtensions
{
    public static object TryClone(this object self, bool deep = false)
    {
        if (self is ICloneable cloneable)
            return cloneable.Clone();

        if (!deep) return self;
        
        var settings = new JsonSerializerOptions { UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode, WriteIndented = false};
        var json = JsonSerializer.Serialize(self, settings);
        return JsonSerializer.Deserialize(json, self.GetType(), settings)!;
    }
}