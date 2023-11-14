using Microsoft.AspNetCore.Http;

namespace Common.Api.Authentication;

public class ApiKeyAuthenticationEndpointFilter : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";
    
    private readonly IApiKeyValidator _apiKeyValidator;

    public ApiKeyAuthenticationEndpointFilter(IApiKeyValidator apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string? apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (!_apiKeyValidator.IsValid(apiKey))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }
}