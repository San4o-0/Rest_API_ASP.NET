using Domain.Cars;

namespace Application.Common.Interfaces.Repositories;

public interface ICarRepository
{
    Task<Car?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Car?> GetByVin(string vin, CancellationToken cancellationToken);
    Task<Car> Add(Car car, CancellationToken cancellationToken);
    Task<Car> Update(Car car, CancellationToken cancellationToken);
    Task Delete(Car car, CancellationToken cancellationToken);
}
