namespace SampleApiWithTestContainers.Product;

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDtTm { get; init; }
    
    private Product(Guid id, string name, DateTimeOffset createdDtTm)
    {
        Id = id;
        Name = name;
        CreatedDtTm = createdDtTm;
    }

    public static Product Create(string name, DateTimeOffset createdDtTm) =>
        new(Guid.NewGuid(), name, createdDtTm);
}