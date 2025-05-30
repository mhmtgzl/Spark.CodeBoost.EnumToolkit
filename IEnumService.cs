namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Provides functionality to list enums and retrieve their values with localized descriptions.
/// </summary>
public interface IEnumService
{
    /// <summary>
    /// Returns a list of enum values for the given enum type name, including localized descriptions.
    /// </summary>
    /// <param name="enumName">The name of the enum type (e.g., "OrderStatus").</param>
    /// <param name="culture">Optional culture code (e.g., "en", "tr"). Defaults to "tr".</param>
    /// <returns>List of enum values with names, integer values, and localized descriptions.</returns>
    List<EnumValueDto> GetEnumValues(string enumName, string? culture = "tr");

    /// <summary>
    /// Returns a list of all available enum type names that can be queried.
    /// </summary>
    /// <returns>List of enum type names as strings.</returns>
    List<string> GetAvailableEnums();
}
