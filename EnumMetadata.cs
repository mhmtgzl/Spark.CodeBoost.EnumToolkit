namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Holds metadata about an enum type, including its name and reflection Type reference.
/// Used internally to track available enums for API access, localization, or synchronization.
/// </summary>
public class EnumMetadata
{
    /// <summary>
    /// The name of the enum type (e.g., "OrderStatus").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The .NET <see cref="Type"/> representing the enum.
    /// </summary>
    public Type Type { get; set; }
}
