using SampleApi.Common.Core.Repository;

namespace SampleApi.Products.Core;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetExistingProductByName(string name);
}