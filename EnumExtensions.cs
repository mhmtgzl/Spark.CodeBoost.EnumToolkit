using System.ComponentModel;
using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumValue, string culture = "tr") where T : Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());

        // Önce LocalizedDescription'a bak
        var localized = field?
            .GetCustomAttributes<LocalizedDescriptionAttribute>()
            .FirstOrDefault(a => a.Language.Equals(culture, StringComparison.OrdinalIgnoreCase));

        if (localized != null)
            return localized.Description;

        // Description varsa (eski yapı)
        var descriptionAttr = field?
            .GetCustomAttribute<DescriptionAttribute>();

        if (descriptionAttr != null)
            return descriptionAttr.Description;

        // Hiçbiri yoksa enum adı
        return enumValue.ToString();
    }
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
