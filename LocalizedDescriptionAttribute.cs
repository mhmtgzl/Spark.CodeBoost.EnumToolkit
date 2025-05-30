namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Allows assigning localized (language-specific) descriptions to enum fields.
/// This enables displaying enum values in different languages.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class LocalizedDescriptionAttribute : Attribute
{
    /// <summary>
    /// The language code (e.g., "en" for English, "tr" for Turkish).
    /// </summary>
    public string Language { get; }

    /// <summary>
    /// The localized description for the enum value.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Creates a new LocalizedDescriptionAttribute with the specified language and description.
    /// </summary>
    /// <param name="language">Language code (e.g., "en", "tr").</param>
    /// <param name="description">The localized description text.</param>
    public LocalizedDescriptionAttribute(string language, string description)
    {
        Language = language;
        Description = description;
    }
}
