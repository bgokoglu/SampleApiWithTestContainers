using SampleApi.Common.Core;

namespace SampleApi.Products.Api;

public static class ProductApiPaths
{
    internal const string GetAll = $"{ApiPaths.Root}/products";
    internal const string GetById = $"{ApiPaths.Root}/products/{{id}}";
    internal const string Create = $"{ApiPaths.Root}/products";
    internal const string Update = $"{ApiPaths.Root}/products/{{id}}";
    internal const string Delete = $"{ApiPaths.Root}/products/{{id}}";
}