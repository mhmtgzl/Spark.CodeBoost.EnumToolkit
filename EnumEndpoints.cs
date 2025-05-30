using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Spark.CodeBoost.EnumToolkit;
public static class EnumEndpoints
{
    public static void MapEnumEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Enums").WithTags("Enums");

        // Tüm enum türlerini listeleme
        group.MapGet("types", async (IEnumService enumService) =>
        {
            var enums = enumService.GetAvailableEnums();
            return Results.Ok(enums);
        });

        // Belirli bir enum türünü alma
        group.MapGet("", async ([FromQuery] string enumName, [FromQuery] string? language, IEnumService enumService) =>
        {
            try
            {
                var result = enumService.GetEnumValues(enumName, language);
                return Results.Ok(result);
            }
            catch (ArgumentException)
            {
                return Results.BadRequest("Invalid enum type.");
            }
        });
    }
}
