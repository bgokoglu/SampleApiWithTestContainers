using SampleApiWithTestContainers.Infrastructure;

namespace SampleApiWithTestContainers.Product;

public interface IProductRepository : IRepository<Product>
{
}