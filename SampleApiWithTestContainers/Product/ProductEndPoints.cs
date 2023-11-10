namespace SampleApiWithTestContainers.Product;

public static class ProductEndPoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet("/api/product", async context =>
        {
            var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
            var products = await productRepository.GetAll();
            await context.Response.WriteAsJsonAsync(products);
        });

        app.MapGet("/api/product/{id}", async context =>
        {
            var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
            var id = context.Request.RouteValues["id"] as string;

            if (Guid.TryParse(id, out var productId))
            {
                var product = await productRepository.GetById(productId);

                if (product != null)
                {
                    await context.Response.WriteAsJsonAsync(product);
                    return;
                }
            }

            context.Response.StatusCode = 404; // Not Found
            await context.Response.WriteAsync("Product not found.");
        });

        app.MapPost("/api/product", async context =>
        {
            var productRepository = context.RequestServices.GetRequiredService<IProductRepository>();
            var product = await context.Request.ReadFromJsonAsync<Product>();

            if (product != null)
            {
                await productRepository.Add(product);
                context.Response.StatusCode = 201; // Created
                await context.Response.WriteAsJsonAsync(product);
            }
            else
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Invalid product data.");
            }
        });

        app.MapPut("/api/product/{id}", async context =>
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

        app.MapDelete("/api/product/{id}", async context =>
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