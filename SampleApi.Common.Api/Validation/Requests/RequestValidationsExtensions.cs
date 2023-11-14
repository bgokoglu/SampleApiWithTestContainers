using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApi.Common.Api.Validation.Requests;

public static class RequestValidationsExtensions
{
    public static IServiceCollection AddRequestsValidations(this IServiceCollection services, Assembly assembly) =>
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
}
