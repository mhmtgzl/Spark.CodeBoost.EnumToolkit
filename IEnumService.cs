namespace Spark.CodeBoost.EnumToolkit;

public interface IEnumService
{
    List<EnumValueDto> GetEnumValues(string enumName, string? culture = "tr");
    List<string> GetAvailableEnums();
}
