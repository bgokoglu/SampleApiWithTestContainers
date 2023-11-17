using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using SampleApi.Common.Api.Authentication;
using SampleApi.Common.Api.Validation.Requests;
using SampleApi.Common.Core.SystemClock;
using SampleApi.Common.Infrastructure.Data;
using SampleApi.Products.Core;
using SampleApi.Products.Infrastructure.Database;
using SampleApi.Products.IntegrationEvents;

namespace SampleApi.Products.Api;

public static class ProductEndPoints
{
    public static void MapProducts(this WebApplication app)
    {
        app.MapGet(ProductApiPaths.GetAll, (IProductRepository repository) =>
            {
                var products = repository.GetAll();
                return Results.Ok(products.ToList().ConvertAll(p => p.MapToProductResponse()));
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Returns all products",
                Description = "This endpoint is used to retrieve all existing products."
            })
            .Produces<IEnumerable<ProductResponse>>()
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet(ProductApiPaths.GetById, (Guid id,
                IProductRepository repository) =>
            {
                var product = repository.GetById(id);
                return product is not null ? Results.Ok(product.MapToProductResponse()) : Results.NotFound();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Returns product",
                Description = "This endpoint is used to get the product."
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost(ProductApiPaths.Create, async (ProductRequest request,
                IProductRepository repository,
                IUnitOfWork<ProductPersistence> unitOfWork,
                ISystemClock systemClock) =>
            {
                var existingProduct = await repository.GetExistingProductByName(request.Name!);
                var newProduct = Product.Create(request.Name!, systemClock.Now, existingProduct?.Name);

                await unitOfWork.BeginTransactionAsync();

                repository.Add(newProduct);
                unitOfWork.AddEvent(ProductCreatedEvent.Create(newProduct.Id));

                await unitOfWork.CompleteAsync();

                return Results.Created($"/{ProductApiPaths.GetById}/{newProduct.Id}", newProduct.Id);
            })
            .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
            .ValidateRequest<ProductRequest>()
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Creates a product",
                Description = "This endpoint is used to create a product."
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut(ProductApiPaths.Update, (Guid id,
                    ProductRequest request,
                    IProductRepository repository,
                    ISystemClock systemClock) =>
                {
                    var existingProduct = repository.GetById(id);

                    if (existingProduct is null)
                        return Results.NotFound();

                    existingProduct.Name = request.Name!;
                    existingProduct.CreatedDtTm = systemClock.Now;
                    repository.Update(existingProduct);

                    return Results.NoContent();
                })
            .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
            .ValidateRequest<ProductRequest>()
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Updates product",
                Description = "This endpoint is used to update the product."
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete(ProductApiPaths.Delete,
                (Guid id,
                    IProductRepository repository) =>
                {
                    var product = repository.GetById(id);

                    if (product is null)
                        return Results.NotFound();

                    repository.Delete(id);
                    return Results.NoContent();
                })
            .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Deletes product",
                Description = "This endpoint is used to delete the product."
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}