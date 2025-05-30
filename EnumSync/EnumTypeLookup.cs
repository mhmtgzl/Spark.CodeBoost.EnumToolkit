namespace Spark.CodeBoost.EnumToolkit.EnumSync;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Represents a database record for a specific enum value and its localized description.
/// Used to store enum definitions with their translations in a structured format.
/// </summary>
[Table("EnumTypeLookups", Schema = "core")]
public class EnumTypeLookup
{
    /// <summary>
    /// Primary key identifier for the record.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The name of the enum type (e.g., "OrderStatus").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EnumName { get; set; } = null!;

    /// <summary>
    /// The integer value of the enum field (e.g., 0, 1, 2).
    /// </summary>
    [Required]
    public int EnumValue { get; set; }

    /// <summary>
    /// The name of the enum member (e.g., "Pending", "Completed").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The localized description of the enum member in the specified language.
    /// </summary>
    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// The language code of the description (e.g., "en", "tr").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string Language { get; set; } = null!;
}
