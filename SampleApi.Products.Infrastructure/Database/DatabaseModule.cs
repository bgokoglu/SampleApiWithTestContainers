using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Common.Api.Validation.Requests;
using SampleApi.Common.Core.Configuration;
using SampleApi.Common.Infrastructure.Data;
using SampleApi.Products.Core;
using SampleApi.Products.Infrastructure.Database.Repositories;

namespace SampleApi.Products.Infrastructure.Database;

public static class DatabaseModule
{
    private static Assembly CurrentModule => typeof(ProductRequest).Assembly;
    
    internal static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddRequestsValidations(CurrentModule);

        return services;
    }
    
    internal static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }
    
    internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddDbContextFactory<ProductPersistence>(options =>
        //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var databaseConfigOptions = configuration.GetSection(DatabaseConfigOptions.Key).Get<DatabaseConfigOptions>();
        
        services.AddDbContext<ProductPersistence>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), action =>
            {
                action.CommandTimeout(databaseConfigOptions!.CommandTimeout);
            });
            options.EnableDetailedErrors(databaseConfigOptions!.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseConfigOptions.EnableSensitiveDataLogging);
        }, optionsLifetime: ServiceLifetime.Singleton);
        
        services.AddScoped<IUnitOfWork<ProductPersistence>, UnitOfWork<ProductPersistence>>();

        return services;
    }
    
    internal static IApplicationBuilder UseDatabase(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductPersistence>();
        context.Database.Migrate();

        return applicationBuilder;
    }
}