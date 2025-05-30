using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Provides minimal API endpoints to expose enums and their localized values over HTTP.
/// </summary>
public static class EnumEndpoints
{
    /// <summary>
    /// Registers enum-related endpoints under the "Enums" route group.
    /// Adds:
    /// - GET /Enums/types → Returns available enum type names
    /// - GET /Enums?enumName=...&language=... → Returns localized values for a specific enum
    /// </summary>
    /// <param name="app">The route builder used to map endpoints.</param>
    public static void MapEnumEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Enums").WithTags("Enums");

        /// <summary>
        /// GET /Enums/types  
        /// Returns a list of all enum type names that are available in the system.
        /// </summary>
        group.MapGet("types", async (IEnumService enumService) =>
        {
            var enums = enumService.GetAvailableEnums();
            return Results.Ok(enums);
        });

        /// <summary>
        /// GET /Enums?enumName=OrderStatus&language=tr  
        /// Returns a list of localized values for the specified enum.
        /// Language is optional; defaults to current user language or "tr".
        /// </summary>
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
