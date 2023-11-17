using SampleApi.Common.Infrastructure.Data.Repository;

namespace SampleApi.Products.Core;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetExistingProductByName(string name);
}