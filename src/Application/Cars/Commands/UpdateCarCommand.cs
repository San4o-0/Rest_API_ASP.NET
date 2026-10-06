using Application.Common.Interfaces.Repositories;
using Domain.Cars;
using MediatR;

namespace Application.Cars.Commands;

public record UpdateCarCommand : IRequest<Car?>
{
    public required Guid CarId { get; init; }
    public required string Vin { get; init; }
    public required string Brand { get; init; }
    public required string Model { get; init; }
    public required int Year { get; init; }
    public required decimal Price { get; init; }
    public required FuelType FuelType { get; init; }
    public required bool IsSold { get; init; }
}

public class UpdateCarCommandHandler(ICarRepository carRepository)
    : IRequestHandler<UpdateCarCommand, Car?>
{
    public async Task<Car?> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
        var car = await carRepository.GetById(request.CarId, cancellationToken);
        if (car is null)
        {
            return null;
        }

        var carWithSameVin = await carRepository.GetByVin(request.Vin, cancellationToken);
        if (carWithSameVin is not null && carWithSameVin.Id != request.CarId)
        {
            throw new ArgumentException($"Car with VIN '{request.Vin}' already exists");
        }

        car.UpdateDetails(
            request.Vin,
            request.Brand,
            request.Model,
            request.Year,
            request.Price,
            request.FuelType,
            request.IsSold);

        return await carRepository.Update(car, cancellationToken);
    }
}
