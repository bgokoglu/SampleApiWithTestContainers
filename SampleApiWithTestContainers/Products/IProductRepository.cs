using Common.Core.Repository;

namespace SampleApiWithTestContainers.Products;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetExistingProductByName(string name);
}