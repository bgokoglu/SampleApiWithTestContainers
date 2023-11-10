using Common.Core.Repository;

namespace SampleApiWithTestContainers.Product;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetExistingProductByName(string name);
}