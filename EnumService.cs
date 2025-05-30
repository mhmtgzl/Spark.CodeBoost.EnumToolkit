using Spark.CodeBoost.Common;

namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Provides runtime access to enum types and their localized values.
/// Supports language-aware conversion of enums to DTOs for use in UI or APIs.
/// </summary>
public class EnumService : IEnumService
{
    private readonly IReadOnlyList<EnumMetadata> _enumMetadata;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Creates a new instance of <see cref="EnumService"/>.
    /// </summary>
    /// <param name="enumMetadata">List of registered enum types and their metadata.</param>
    /// <param name="currentUser">Provides access to the current user's language settings.</param>
    public EnumService(IReadOnlyList<EnumMetadata> enumMetadata, ICurrentUser currentUser)
    {
        _enumMetadata = enumMetadata;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Returns a list of available enum type names that are registered in the system.
    /// </summary>
    /// <returns>List of enum type names as strings.</returns>
    public List<string> GetAvailableEnums()
    {
        return _enumMetadata.Select(e => e.Name).ToList();
    }

    /// <summary>
    /// Returns the values of a given enum type as a list of <see cref="EnumValueDto"/>,
    /// with descriptions translated to the given or current user's language.
    /// </summary>
    /// <param name="enumName">The name of the enum type (e.g., "OrderStatus").</param>
    /// <param name="language">Optional language code (e.g., "en", "tr"). If null, uses current user's language.</param>
    /// <returns>List of <see cref="EnumValueDto"/> representing enum values and their descriptions.</returns>
    /// <exception cref="ArgumentException">Thrown if the enum name is invalid or not found.</exception>
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
