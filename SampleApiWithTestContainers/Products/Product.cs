using System.Runtime.CompilerServices;
using Common.Core.BusinessRules;

namespace SampleApiWithTestContainers.Products;

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDtTm { get; set; }
    
    private Product(Guid id, string name, DateTimeOffset createdDtTm)
    {
        Id = id;
        Name = name;
        CreatedDtTm = createdDtTm;
    }
    
    public static Product Create(string name, DateTimeOffset createdDtTm, string? existingName)
    {
        BusinessRuleValidator.Validate(new ProductCanBeCreatedOnlyForNonExistingRule(name, existingName));
        return new Product(Guid.NewGuid(), name, createdDtTm);
    }

    public ProductResponse MapToProductResponse()
    {
        return new ProductResponse
        {
            Id = Id,
            Name = Name,
            CreatedDtTm = CreatedDtTm
        };
    }
}