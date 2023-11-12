using Microsoft.Extensions.DependencyInjection;

namespace Common.Api.Authentication;

public static class CommonApiAuthenticationModule
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

        return services;
    }
}