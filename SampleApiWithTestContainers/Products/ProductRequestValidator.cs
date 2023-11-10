using FluentValidation;

namespace SampleApiWithTestContainers.Products;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}