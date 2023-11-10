using Common.Core.Repository;

namespace SampleApiWithTestContainers.Product;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ProductPersistence context) : base(context)
    {
    }
}