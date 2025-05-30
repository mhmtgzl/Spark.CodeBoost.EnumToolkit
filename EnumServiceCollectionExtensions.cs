using Microsoft.Extensions.DependencyInjection;
using Spark.CodeBoost.Common;
using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit;

public static class EnumServiceCollectionExtensions
{
    public static IServiceCollection AddEnumServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        var enumTypes = EnumCollector.CollectEnumsFromAssemblies(assemblies);

        var metadata = enumTypes.Select(t => new EnumMetadata
        {
            Name = t.Name,
            Type = t
        }).ToList();

        services.AddSingleton(metadata); // BURADA sadece List<EnumMetadata> olarak ekliyoruz!

        services.AddScoped<IEnumService>(sp =>
        {
            var enumMetadata = sp.GetRequiredService<List<EnumMetadata>>(); // BURADA alıyoruz
            var currentUser = sp.GetRequiredService<ICurrentUser>();
            return new EnumService(enumMetadata, currentUser);
        });

        return services;
    }
}
