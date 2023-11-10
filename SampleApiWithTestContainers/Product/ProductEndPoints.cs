using Common.Api.Validation.Requests;
using Microsoft.OpenApi.Models;
using Common.Core.SystemClock;

namespace SampleApiWithTestContainers.Product;

public static class ProductEndPoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet(ProductApiPaths.GetAll, async (IProductRepository repository) =>
            {
                var products = await repository.GetAll();
                return Results.Ok(products);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Returns all products",
                Description = "This endpoint is used to retrieve all existing passes.",
            })
            .Produces<IEnumerable<Product>>()
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet(ProductApiPaths.GetById, async (Guid id, IProductRepository repository) =>
            {
                var product = await repository.GetById(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
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
                async (ProductRequest request, IProductRepository repository, ISystemClock clock) =>
                {
                    var existingProduct = await repository.GetExistingProductByName(request.Name);
                    var newProduct = Product.Create(request.Name, clock.Now, existingProduct?.Name);
                    await repository.Add(newProduct);
                    return Results.Created($"/{ProductApiPaths.GetById}/{newProduct.Id}", newProduct.Id);
                })
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
                async (Guid id, ProductRequest request, IProductRepository repository, ISystemClock clock) =>
                {
                    var existingProduct = await repository.GetById(id);

                    if (existingProduct != null)
                    {
                        existingProduct.Name = request.Name;
                        existingProduct.CreatedDtTm = clock.Now;
                        await repository.Update(existingProduct);
                        Results.NoContent();
                    }

                    Results.NotFound();
                })
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

                if (product != null)
                {
                    await repository.Delete(id);
                    Results.NoContent();
                }

                Results.NotFound();
            })
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