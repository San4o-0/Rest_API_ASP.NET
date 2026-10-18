using Application.Cars.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Cars;
using LanguageExt;
using MediatR;

namespace Application.Cars.Commands;

public record DeleteCarCommand : IRequest<Either<CarException, Car>>
{
    public required Guid CarId { get; init; }
}

public class DeleteCarCommandHandler(ICarRepository carRepository)
    : IRequestHandler<DeleteCarCommand, Either<CarException, Car>>
{
    public async Task<Either<CarException, Car>> Handle(
        DeleteCarCommand request,
        CancellationToken cancellationToken)
    {
        var carId = new CarId(request.CarId);
        var car = await carRepository.GetById(carId, cancellationToken);

        return await car.MatchAsync(
            c => DeleteEntity(c, cancellationToken),
            () => new CarNotFoundException(carId));
    }

    private async Task<Either<CarException, Car>> DeleteEntity(
        Car car,
        CancellationToken cancellationToken)
    {
        try
        {
            await carRepository.Delete(car, cancellationToken);
            return car;
        }
        catch (Exception exception)
        {
            return new CarUnhandledException(car.Id, exception);
        }
    }
}
