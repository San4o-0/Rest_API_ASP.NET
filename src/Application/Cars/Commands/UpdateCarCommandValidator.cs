using FluentValidation;

namespace Application.Cars.Commands;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.CarId).NotEmpty();
        RuleFor(x => x.Vin).NotEmpty().MaximumLength(17);
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Year).InclusiveBetween(1900, 2100);
        RuleFor(x => x.Price).InclusiveBetween(0.01m, 100_000_000m);
        RuleFor(x => x.FuelType).IsInEnum();
        RuleFor(x => x.IsSold).NotNull();
    }
}
