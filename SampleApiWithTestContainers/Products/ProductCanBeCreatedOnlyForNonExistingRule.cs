using Common.Core.BusinessRules;

namespace SampleApiWithTestContainers.Products;

public sealed class ProductCanBeCreatedOnlyForNonExistingRule : IBusinessRule
{
    private readonly string _name;
    private readonly string? _existingName;

    internal ProductCanBeCreatedOnlyForNonExistingRule(string name, string? existingName)
    {
        _name = name;
        _existingName = existingName;
    }

    public bool IsMet() => !_name.Equals(_existingName, StringComparison.OrdinalIgnoreCase);

    public string Error => "Product with the same name already exists";
}