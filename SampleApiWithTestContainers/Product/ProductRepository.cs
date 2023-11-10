using Common.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace SampleApiWithTestContainers.Product;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ProductPersistence _context;

    public ProductRepository(ProductPersistence context) : base(context)
    {
        _context = context;
    }

    public Task<Product?> GetExistingProductByName(string name)
    {
        return _context.Set<Product>().Where(p => p.Name.Equals(name)).FirstOrDefaultAsync();
    }
}