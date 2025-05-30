using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit;

/// <summary>
/// Provides functionality to discover all enum types from given .NET assemblies.
/// Useful for automatically registering or processing enums at runtime.
/// </summary>
public static class EnumCollector
{
    /// <summary>
    /// Scans the specified assemblies and returns all types that are enums.
    /// </summary>
    /// <param name="assemblies">One or more assemblies to search for enum types.</param>
    /// <returns>A list of enum types found in the provided assemblies.</returns>
    public static List<Type> CollectEnumsFromAssemblies(params Assembly[] assemblies)
    {
        return assemblies.SelectMany(assembly => assembly.GetTypes())
                         .Where(type => type.IsEnum)
                         .ToList();
    }
}
