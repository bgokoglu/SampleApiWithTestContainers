using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace SampleApiWithTestContainers.Product;

public static class ProductEndPoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet(ProductApiPaths.GetAll, async (IProductRepository repository) =>
            {
                //var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
                var products = await repository.GetAll();
                //await context.Response.WriteAsJsonAsync(products);
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
                // var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
                // var id = context.Request.RouteValues["id"] as string;
                //
                // if (Guid.TryParse(id, out var productId))
                // {
                //     var product = await productRepository.GetById(productId);
                //
                //     if (product != null)
                //     {
                //         await context.Response.WriteAsJsonAsync(product);
                //         return;
                //     }
                // }
                //
                // context.Response.StatusCode = 404; // Not Found
                // await context.Response.WriteAsync("Product not found.");

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
                var product = Product.Create(request.Name, clock.UtcNow);
                await repository.Add(product);
                return Results.Created($"/{ProductApiPaths.GetById}/{product.Id}", product.Id);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Creates a product",
                Description = "This endpoint is used to create a product.",
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut(ProductApiPaths.Update, async context =>
        {
            var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
            var id = context.Request.RouteValues["id"] as string;

            if (Guid.TryParse(id, out var productId))
            {
                var existingProduct = await productRepository.GetById(productId);

                if (existingProduct != null)
                {
                    var updatedProduct = await context.Request.ReadFromJsonAsync<Product>();

                    if (updatedProduct != null)
                    {
                        updatedProduct.Id = productId;
                        await productRepository.Update(updatedProduct);
                        context.Response.StatusCode = 204; // No Content
                        return;
                    }
                }
            }

            context.Response.StatusCode = 404; // Not Found
            await context.Response.WriteAsync("Product not found.");
        });

        app.MapDelete(ProductApiPaths.Delete, async context =>
        {
            var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
            var id = context.Request.RouteValues["id"] as string;

            if (Guid.TryParse(id, out var productId))
            {
                var product = await productRepository.GetById(productId);

                if (product != null)
                {
                    await productRepository.Delete(productId);
                    context.Response.StatusCode = 204; // No Content
                    return;
                }
            }

            context.Response.StatusCode = 404; // Not Found
            await context.Response.WriteAsync("Product not found.");
        });
    }
}