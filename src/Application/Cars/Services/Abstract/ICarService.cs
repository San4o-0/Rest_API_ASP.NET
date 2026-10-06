using Application.Cars.Filters;
using Domain.Cars;

namespace Application.Cars.Services.Abstract;

public interface ICarService
{
    Task<IReadOnlyList<Car>> GetCars(CancellationToken cancellationToken);
    Task<IReadOnlyList<Car>> SearchCars(CarFilter filter, CancellationToken cancellationToken);
    Task<Car?> GetCar(Guid id, CancellationToken cancellationToken);

    Task<Car> Add(
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType,
        CancellationToken cancellationToken);

    Task<Car?> Update(
        Guid id,
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType,
        bool isSold,
        CancellationToken cancellationToken);

    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}
