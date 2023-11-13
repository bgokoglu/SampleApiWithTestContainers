using System.Reflection;
using Common.Api.Validation.Requests;
using Common.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SampleApiWithTestContainers.Products;

public static class ProductModule
{
    private static Assembly CurrentModule => typeof(ProductRequest).Assembly;
    
    public static IServiceCollection AddProductValidations(this IServiceCollection services)
    {
        services.AddRequestsValidations(CurrentModule);

        return services;
    }
    
    public static IServiceCollection AddProductRepository(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddProductDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddDbContextFactory<ProductPersistence>(options =>
        //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var databaseConfigOptions = configuration.GetSection(DatabaseConfigOptions.Key).Get<DatabaseConfigOptions>();
        
        services.AddDbContext<ProductPersistence>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), action =>
            {
                action.CommandTimeout(databaseConfigOptions.CommandTimeout);
            });
            options.EnableDetailedErrors(databaseConfigOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseConfigOptions.EnableSensitiveDataLogging);
        }, optionsLifetime: ServiceLifetime.Singleton);

        return services;
    }
    
    public static IApplicationBuilder UseProductDatabase(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductPersistence>();
        context.Database.Migrate();

        return applicationBuilder;
    }
}