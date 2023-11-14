using Common.Api.Authentication;
using Common.Api.Validation.Requests;
using Common.Core.SystemClock;
using Common.Infrastructure.Events.EventBus;
using Microsoft.OpenApi.Models;

namespace SampleApiWithTestContainers.Products;

public static class ProductEndPoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet(ProductApiPaths.GetAll, async (IProductRepository repository) =>
            {
                var products = await repository.GetAll();
                return Results.Ok(products.ToList().ConvertAll(p => p.MapToProductResponse()));
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Returns all products",
                Description = "This endpoint is used to retrieve all existing passes.",
            })
            .Produces<IEnumerable<ProductResponse>>()
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet(ProductApiPaths.GetById, async (Guid id, IProductRepository repository) =>
            {
                var product = await repository.GetById(id);
                return product is not null ? Results.Ok(product.MapToProductResponse()) : Results.NotFound();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Returns product",
                Description = "This endpoint is used to get the product.",
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost(ProductApiPaths.Create,
                async (ProductRequest request, IProductRepository repository, ISystemClock systemClock,
                    IEventBus eventBus) =>
                {
                    var existingProduct = await repository.GetExistingProductByName(request.Name!);
                    var newProduct = Product.Create(request.Name!, systemClock.Now, existingProduct?.Name);
                    await repository.Add(newProduct);

                    var productCreatedEvent = ProductCreatedEvent.Create(newProduct.Id);
                    await eventBus.PublishAsync(productCreatedEvent);

                    return Results.Created($"/{ProductApiPaths.GetById}/{newProduct.Id}", newProduct.Id);
                })
            .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
            .ValidateRequest<ProductRequest>()
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Creates a product",
                Description = "This endpoint is used to create a product.",
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut(ProductApiPaths.Update,
                async (Guid id, ProductRequest request, IProductRepository repository, ISystemClock systemClock) =>
                {
                    var existingProduct = await repository.GetById(id);

                    if (existingProduct is null)
                        return Results.NotFound();

                    existingProduct.Name = request.Name!;
                    existingProduct.CreatedDtTm = systemClock.Now;
                    await repository.Update(existingProduct);

                    return Results.NoContent();
                })
            .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
            .ValidateRequest<ProductRequest>()
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Updates product",
                Description = "This endpoint is used to update the product.",
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete(ProductApiPaths.Delete, async (Guid id, IProductRepository repository) =>
            {
                var product = await repository.GetById(id);

                if (product is null)
                    return Results.NotFound();

                await repository.Delete(id);
                return Results.NoContent();
            })
            .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Deletes product",
                Description = "This endpoint is used to delete the product.",
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}