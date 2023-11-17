using Microsoft.EntityFrameworkCore;
using SampleApi.Common.Infrastructure.Data.Repository;
using SampleApi.Products.Core;

namespace SampleApi.Products.Infrastructure.Database.Repositories;

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