using Common.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SampleApiWithTestContainers.Products;

public sealed class ProductPersistence : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    public ProductPersistence(DbContextOptions<ProductPersistence> options, IServiceProvider serviceProvider) : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    public DbSet<Product> Products => Set<Product>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Products");
        
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");
        
        modelBuilder.Entity<Product>().Property(pass => pass.Name).IsRequired();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new SlowQueryInterceptor(_serviceProvider));
        
        base.OnConfiguring(optionsBuilder);
    }
}