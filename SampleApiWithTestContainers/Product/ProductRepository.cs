using SampleApiWithTestContainers.Infrastructure;

namespace SampleApiWithTestContainers.Product;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ProductDbContext context) : base(context)
    {
    }
}