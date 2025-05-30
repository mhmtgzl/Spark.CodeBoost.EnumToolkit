using Spark.CodeBoost.Common;

namespace Spark.CodeBoost.EnumToolkit;

public class EnumService : IEnumService
{
    private readonly IReadOnlyList<EnumMetadata> _enumMetadata;
    private readonly ICurrentUser _currentUser;

    public EnumService(IReadOnlyList<EnumMetadata> enumMetadata, ICurrentUser currentUser)
    {
        _enumMetadata = enumMetadata;
        _currentUser = currentUser;
    }

    public List<string> GetAvailableEnums()
    {
        return _enumMetadata.Select(e => e.Name).ToList();
    }

    public List<EnumValueDto> GetEnumValues(string enumName, string? language = null)
    {
        var enumType = _enumMetadata
            .FirstOrDefault(e => e.Name.Equals(enumName, StringComparison.OrdinalIgnoreCase))
            ?.Type;

        if (enumType == null || !enumType.IsEnum)
            throw new ArgumentException("Invalid enum type.");

        var userLang = _currentUser.Language ?? "tr";

        var headerValue = userLang.Length >= 2
            ? userLang.ToLower().Substring(0, 2)
            : "tr";

        var finalCulture = language ?? headerValue ?? "tr";

        var method = typeof(EnumExtensions)
            .GetMethod(nameof(EnumExtensions.ToEnumValueDtoList), new[] { typeof(string) })?
            .MakeGenericMethod(enumType);

        return (List<EnumValueDto>)method?.Invoke(null, new object[] { finalCulture })!;
    }
}
