using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit;

public static class EnumCollector
{
    public static List<Type> CollectEnumsFromAssemblies(params Assembly[] assemblies)
    {
        return assemblies.SelectMany(assembly => assembly.GetTypes())
                         .Where(type => type.IsEnum)
                         .ToList();
    }
}
