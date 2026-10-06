using System.ComponentModel.DataAnnotations;
using Domain.Cars;

namespace Api.Dtos;

public record CarDto(
    Guid Id,
    string Vin,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    FuelType FuelType,
    bool IsSold,
    DateTime CreatedAt)
{
    public static CarDto FromDomainModel(Car car)
        => new(car.Id, car.Vin, car.Brand, car.Model, car.Year, car.Price, car.FuelType, car.IsSold, car.CreatedAt);
}

public record CreateCarDto(
    string Vin,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    FuelType FuelType);

public record CarFilterDto(
    [Range(0, 100_000_000)] decimal? MinPrice,
    [Range(0, 100_000_000)] decimal? MaxPrice,
    [Range(1900, 2100)] int? MinYear,
    [Range(1900, 2100)] int? MaxYear) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPrice is not null && MaxPrice is not null && MinPrice > MaxPrice)
        {
            yield return new ValidationResult(
                "MinPrice cannot be greater than MaxPrice",
                [nameof(MinPrice), nameof(MaxPrice)]);
        }

        if (MinYear is not null && MaxYear is not null && MinYear > MaxYear)
        {
            yield return new ValidationResult(
                "MinYear cannot be greater than MaxYear",
                [nameof(MinYear), nameof(MaxYear)]);
        }
    }
}

public record UpdateCarDto(
    string Vin,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    FuelType FuelType,
    bool IsSold);
