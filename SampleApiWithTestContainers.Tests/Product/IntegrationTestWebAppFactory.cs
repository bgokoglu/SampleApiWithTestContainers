using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleApiWithTestContainers.Products;
using Testcontainers.PostgreSql;

namespace SampleApiWithTestContainers.Tests.Product;

public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("testdb")
        .WithUsername("testusr")
        .WithPassword("testpwd")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptorType = typeof(DbContextOptions<ProductPersistence>);
            var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ProductPersistence>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductPersistence>();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
