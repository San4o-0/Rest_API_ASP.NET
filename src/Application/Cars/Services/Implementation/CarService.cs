using Application.Cars.Filters;
using Application.Cars.Services.Abstract;
using Application.Common.Interfaces;
using Domain.Cars;

namespace Application.Cars.Services.Implementation;

public class CarService(ICarRepository carRepository) : ICarService
{
    public async Task<IReadOnlyList<Car>> GetCars(CancellationToken cancellationToken)
    {
        return await carRepository.GetAll(cancellationToken);
    }

    public async Task<IReadOnlyList<Car>> SearchCars(CarFilter filter, CancellationToken cancellationToken)
    {
        return await carRepository.GetFiltered(filter, cancellationToken);
    }

    public async Task<Car?> GetCar(Guid id, CancellationToken cancellationToken)
    {
        return await carRepository.GetById(id, cancellationToken);
    }

    public async Task<Car> Add(
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType,
        CancellationToken cancellationToken)
    {
        var existingCar = await carRepository.GetByVin(vin, cancellationToken);
        if (existingCar is not null)
        {
            throw new ArgumentException($"Car with VIN '{vin}' already exists");
        }

        var car = Car.New(Guid.NewGuid(), vin, brand, model, year, price, fuelType);
        return await carRepository.Add(car, cancellationToken);
    }

    public async Task<Car?> Update(
        Guid id,
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType,
        bool isSold,
        CancellationToken cancellationToken)
    {
        var car = await carRepository.GetById(id, cancellationToken);
        if (car is null)
        {
            return null;
        }

        var carWithSameVin = await carRepository.GetByVin(vin, cancellationToken);
        if (carWithSameVin is not null && carWithSameVin.Id != id)
        {
            throw new ArgumentException($"Car with VIN '{vin}' already exists");
        }

        car.UpdateDetails(vin, brand, model, year, price, fuelType, isSold);
        return await carRepository.Update(car, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var car = await carRepository.GetById(id, cancellationToken);
        if (car is null)
        {
            return false;
        }

        await carRepository.Delete(car, cancellationToken);
        return true;
    }
}
