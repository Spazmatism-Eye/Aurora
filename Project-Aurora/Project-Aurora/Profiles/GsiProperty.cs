using System;
using System.Collections.Generic;
using AuroraRgb.Utils;

namespace AuroraRgb.Profiles;

/// <summary>The valid types of GSI property.</summary>
public enum GSIPropertyType
{
    None,
    Number,
    Boolean,
    String,
    Enum
}

internal static class GSIPropertyTypeConverter {
    /// <summary>
    /// A set of predicates that determine if the given <see cref="Type"/> is of the given <see cref="GSIPropertyType"/>
    /// </summary>
    private static Dictionary<GSIPropertyType, Func<Type, bool>> predicates = new()
    {
        [GSIPropertyType.None] = _ => false,
        [GSIPropertyType.Enum] = type => type.IsEnum, // Needs to take priority over number, since enums are stored as numbers as so IsNumericType would be true
        [GSIPropertyType.Number] = type => TypeUtils.IsNumericType(type),
        [GSIPropertyType.Boolean] = type => Type.GetTypeCode(type) == TypeCode.Boolean,
        [GSIPropertyType.String] = type => Type.GetTypeCode(type) == TypeCode.String
    };

    /// <summary>
    /// Gets the <see cref="GSIPropertyType"/> for the given <see cref="Type"/>.
    /// </summary>
    public static GSIPropertyType TypeToPropertyType(Type? type) {
        if (type == null) return GSIPropertyType.None;
        foreach (var (propertyType, predicate) in predicates)
            if (predicate(type))
                return propertyType;
        return GSIPropertyType.None;
    }

    /// <summary>
    /// Determines if the given <see cref="Type"/> is valid for the given <see cref="GSIPropertyType"/>.
    /// </summary>
    public static bool IsTypePropertyType(Type type, GSIPropertyType propertyType) => type == null ? false : predicates[propertyType](type);
}