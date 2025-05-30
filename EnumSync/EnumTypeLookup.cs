namespace Spark.CodeBoost.EnumToolkit.EnumSync;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("EnumTypeLookups", Schema = "core")]
public class EnumTypeLookup
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string EnumName { get; set; } = null!;

    [Required]
    public int EnumValue { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string Language { get; set; } = null!;
}
