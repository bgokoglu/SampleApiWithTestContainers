using Microsoft.AspNetCore.Builder;

namespace SampleApi.Common.Api.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ExceptionMiddleware>();

        return applicationBuilder;
    }
}