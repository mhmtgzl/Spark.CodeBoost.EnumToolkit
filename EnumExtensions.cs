using System.ComponentModel;
using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Provides extension methods for working with enums, including localized description retrieval and conversion to DTO list.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Retrieves the localized description of an enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumValue">The enum value to get the description for.</param>
    /// <param name="culture">The language code (e.g., "en", "tr"). Default is "tr".</param>
    /// <returns>The localized description if available; otherwise, the default name of the enum value.</returns>
    public static string GetDescription<T>(this T enumValue, string culture = "tr") where T : Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());

        // First check for LocalizedDescriptionAttribute
        var localized = field?
            .GetCustomAttributes<LocalizedDescriptionAttribute>()
            .FirstOrDefault(a => a.Language.Equals(culture, StringComparison.OrdinalIgnoreCase));

        if (localized != null)
            return localized.Description;

        // Fallback to [Description] attribute
        var descriptionAttr = field?
            .GetCustomAttribute<DescriptionAttribute>();

        if (descriptionAttr != null)
            return descriptionAttr.Description;

        // Fallback to enum name
        return enumValue.ToString();
    }

    /// <summary>
    /// Converts all values of an enum type into a list of <see cref="EnumValueDto"/>,
    /// including localized descriptions for each value.
    /// </summary>
    /// <typeparam name="T">The enum type to convert.</typeparam>
    /// <param name="culture">The language code (e.g., "en", "tr") used for descriptions. Default is "tr".</param>
    /// <returns>A list of <see cref="EnumValueDto"/> representing the enum values.</returns>
    public static List<EnumValueDto> ToEnumValueDtoList<T>(string culture = "tr") where T : Enum
    {
        return Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .Select(e => new EnumValueDto
                   {
                       Name = e.ToString(),
                       Value = Convert.ToInt32(e),
                       Description = e.GetDescription(culture)
                   })
                   .ToList();
    }
}
