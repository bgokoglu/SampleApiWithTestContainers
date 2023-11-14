using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Common.Api.Validation.Requests;
using SampleApi.Products.Core;
using SampleApi.Products.Infrastructure;

namespace SampleApi.Products.Api;

public static class ProductModule
{
    private static Assembly CurrentModule => typeof(ProductRequest).Assembly;
    
    public static void RegisterProducts(this WebApplication app, string module)
    {
        // if (!app.IsModuleEnabled(module))
        // {
        //     return;
        // }

        app.UseProducts();
        app.MapProducts();
    }

    public static IServiceCollection AddProducts(this IServiceCollection services, IConfiguration configuration,
        string module)
    {
        // if (!services.IsModuleEnabled(module))
        // {
        //     return services;
        // }

        services.AddRequestsValidations(CurrentModule);
        services.AddInfrastructure(configuration);

        return services;
    }

    private static IApplicationBuilder UseProducts(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseInfrastructure();

        return applicationBuilder;
    }
}