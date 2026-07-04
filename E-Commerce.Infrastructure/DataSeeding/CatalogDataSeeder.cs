using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding;

public class CatalogDataSeeder(StoreDbContext context, ILogger<CatalogDataSeeder> logger) : IDataSeeder
{
    public async Task SeedDataAsync(CancellationToken ct = default)
    {
        try
        {
            var pendeingMigrations = await context.Database.GetPendingMigrationsAsync();

            if (pendeingMigrations.Any())
            {
                await context.Database.MigrateAsync(ct);
            }

            var seedRootPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");

            //brands
            await SeedIfEmpty<ProductBrand, int>(seedRootPath, "brands.json", ct);
            //Typs
            await SeedIfEmpty<ProductType, int>(seedRootPath, "types.json", ct);

            await context.SaveChangesAsync(ct);
            //products
            await SeedIfEmpty<Product, int>(seedRootPath, "products.json", ct);

            var count = await context.SaveChangesAsync(ct);

            if (count > 0)
            {
                logger.LogInformation($"{count} Rows Added");
            }
            else
            {
                logger.LogInformation("Database Already Seeded");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
        
    }

    private async Task SeedIfEmpty<TEntity, TKey>(string rootPath, string fileName, CancellationToken ct = default) where TEntity : BaseEntity<TKey>
    {
        if (await context.Set<TEntity>().AnyAsync())
        {
            logger.LogWarning("Table already has data");
            return;
        }

        var filePath = Path.Combine(rootPath, fileName);

        if (!File.Exists(filePath))
        {
            logger.LogWarning($"File {fileName} Not Exist");
            return;
        }

        using var fileStream = File.OpenRead(filePath);

        var options = new JsonSerializerOptions()
        {
           PropertyNameCaseInsensitive = true
        };

        var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(fileStream, options, ct);

        if (data is not null && data.Any())
            await context.Set<TEntity>().AddRangeAsync(data);

    }
}
