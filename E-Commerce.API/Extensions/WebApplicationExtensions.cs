using E_Commerce.Domain.Contracts;

namespace E_Commerce.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SeedAndMigrationDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dataSeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
        var identityDataSeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
       
        await dataSeeder.SeedDataAsync();
        await identityDataSeeder.SeedDataAsync();

        return app;
    }
}
