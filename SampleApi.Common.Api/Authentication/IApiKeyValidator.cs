namespace SampleApi.Common.Api.Authentication;

public interface IApiKeyValidator
{
    bool IsValid(string? apiKey);
}