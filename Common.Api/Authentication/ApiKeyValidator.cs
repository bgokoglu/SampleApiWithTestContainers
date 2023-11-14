using Microsoft.Extensions.Configuration;

namespace Common.Api.Authentication;

public class ApiKeyValidator : IApiKeyValidator
{
    private readonly IConfiguration _configuration;

    public ApiKeyValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public bool IsValid(string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
            return false;

        var keyFromConfig = _configuration.GetValue<string>("Authentication:ApiKey");

        return string.Equals(apiKey, keyFromConfig);
    }
}