using Microsoft.EntityFrameworkCore;

namespace SampleApiWithTestContainers.Products;

public sealed class ProductPersistence : DbContext
{
    public ProductPersistence(DbContextOptions<ProductPersistence> options) : base(options) { }

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
}