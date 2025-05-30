using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit.EnumSync;

/// <summary>
/// Provides functionality to synchronize enums with a database table for multilingual use.
/// Writes or updates enum values into a table such as EnumTypeLookup for use in UI or other systems.
/// </summary>
public class EnumSyncService
{
    private readonly IReadOnlyList<EnumMetadata> _enumMetadataList;

    /// <summary>
    /// Initializes a new instance of <see cref="EnumSyncService"/>.
    /// </summary>
    /// <param name="enumMetadataList">The list of enum metadata used to discover enums.</param>
    public EnumSyncService(IReadOnlyList<EnumMetadata> enumMetadataList)
    {
        _enumMetadataList = enumMetadataList;
    }

    /// <summary>
    /// Synchronizes all discovered enums with the database.
    /// </summary>
    /// <param name="dbContext">The EF Core DbContext used for database access.</param>
    /// <param name="languages">Array of language codes (e.g. "en", "tr") to sync translations for.</param>
    /// <param name="schema">Schema name where EnumTypeLookup table is located. Default is "core".</param>
    /// <param name="deleteOrphans">If true, removes entries that no longer exist in the enum. Default is true.</param>
    public async Task SyncAllAsync(
        DbContext dbContext,
        string[] languages,
        string schema = "core",
        bool deleteOrphans = true)
    {
        foreach (var enumMeta in _enumMetadataList)
        {
            var enumType = enumMeta.Type;

            var method = typeof(EnumSyncService)
                .GetMethod(nameof(SyncEnumAsync), BindingFlags.Instance | BindingFlags.NonPublic)?
                .MakeGenericMethod(enumType);

            if (method is null)
                continue;

            var task = (Task?)method.Invoke(this, new object[]
            {
                dbContext,
                languages,
                schema,
                deleteOrphans
            });

            if (task is not null)
                await task;
        }
    }

    /// <summary>
    /// Synchronizes a single enum type with the database for all specified languages.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to synchronize.</typeparam>
    /// <param name="dbContext">The EF Core DbContext used for database access.</param>
    /// <param name="languages">Array of language codes to include in sync.</param>
    /// <param name="schema">Database schema name.</param>
    /// <param name="deleteOrphans">Whether to delete values from the database that are no longer in the enum.</param>
    private async Task SyncEnumAsync<TEnum>(
        DbContext dbContext,
        string[] languages,
        string schema,
        bool deleteOrphans)
        where TEnum : Enum
    {
        var enumName = typeof(TEnum).Name;
        var dbSet = dbContext.Set<EnumTypeLookup>();

        foreach (var lang in languages)
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

            var newLookups = enumValues.Select(ev => new EnumTypeLookup
            {
                EnumName = enumName,
                EnumValue = Convert.ToInt32(ev),
                Name = ev.ToString()!,
                Description = ev.GetDescription(lang),
                Language = lang
            }).ToList();

            var existing = await dbSet
                .Where(e => e.EnumName == enumName && e.Language == lang)
                .ToListAsync();

            foreach (var lookup in newLookups)
            {
                var match = existing.FirstOrDefault(e => e.EnumValue == lookup.EnumValue);
                if (match == null)
                    await dbSet.AddAsync(lookup);
                else if (match.Description != lookup.Description)
                    match.Description = lookup.Description;
            }

            if (deleteOrphans)
            {
                var toRemove = existing
                    .Where(e => !enumValues.Any(ev => Convert.ToInt32(ev) == e.EnumValue))
                    .ToList();

                if (toRemove.Any())
                    dbSet.RemoveRange(toRemove);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
