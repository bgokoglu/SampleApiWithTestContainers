using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SampleApi.Common.Api.Validation.Requests;

public static class EndpointBuilderExtensions
{
    public static RouteHandlerBuilder ValidateRequest<TRequest>(this RouteHandlerBuilder builder) where TRequest : class =>
        builder.AddEndpointFilter<RequestValidationApiFilter<TRequest>>();
}
