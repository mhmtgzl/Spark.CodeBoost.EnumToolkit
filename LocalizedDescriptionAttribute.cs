namespace Spark.CodeBoost.EnumToolkit;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class LocalizedDescriptionAttribute : Attribute
{
    public string Language { get; }
    public string Description { get; }

    public LocalizedDescriptionAttribute(string language, string description)
    {
        Language = language;
        Description = description;
    }
}
