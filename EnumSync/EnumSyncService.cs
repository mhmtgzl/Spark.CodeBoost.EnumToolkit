using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Spark.CodeBoost.EnumToolkit.EnumSync;

public class EnumSyncService
{
    private readonly IReadOnlyList<EnumMetadata> _enumMetadataList;

    public EnumSyncService(IReadOnlyList<EnumMetadata> enumMetadataList)
    {
        _enumMetadataList = enumMetadataList;
    }

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
