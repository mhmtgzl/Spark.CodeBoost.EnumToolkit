namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Represents a single enum value with its name, numeric value, and localized description.
/// </summary>
public class EnumValueDto
{
    /// <summary>
    /// The name of the enum value as defined in code (e.g., "Pending", "Completed").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The integer value of the enum (e.g., 0, 1, 2).
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// A user-friendly and localized description of the enum value.
    /// This is what should be shown in UI.
    /// </summary>
    public string Description { get; set; }
}
