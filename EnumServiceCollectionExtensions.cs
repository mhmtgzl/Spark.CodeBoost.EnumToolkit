using Microsoft.Extensions.DependencyInjection;
using Spark.CodeBoost.Common;
using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Provides extension methods to register enum-related services into the dependency injection container.
/// </summary>
public static class EnumServiceCollectionExtensions
{
    /// <summary>
    /// Registers enum metadata and services for providing localized enum values at runtime.
    /// This scans the given assemblies for all enum types and makes them available for the system.
    /// </summary>
    /// <param name="services">The application's dependency injection container.</param>
    /// <param name="assemblies">Assemblies to scan for enum types.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddEnumServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        // Scan assemblies to collect all enum types
        var enumTypes = EnumCollector.CollectEnumsFromAssemblies(assemblies);

        // Create metadata for each enum (name + type info)
        var metadata = enumTypes.Select(t => new EnumMetadata
        {
            Name = t.Name,
            Type = t
        }).ToList();

        // Register enum metadata as a singleton so it can be shared across services
        services.AddSingleton(metadata);

        // Register the EnumService with scoped lifetime
        services.AddScoped<IEnumService>(sp =>
        {
            var enumMetadata = sp.GetRequiredService<List<EnumMetadata>>();
            var currentUser = sp.GetRequiredService<ICurrentUser>();
            return new EnumService(enumMetadata, currentUser);
        });

        return services;
    }
}
