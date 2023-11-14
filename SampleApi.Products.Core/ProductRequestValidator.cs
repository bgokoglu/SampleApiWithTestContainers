using FluentValidation;

namespace SampleApi.Products.Core;

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