using Application.Cars.Filters;
using Domain.Cars;

namespace Application.Common.Interfaces.Queries;

public interface ICarQueries
{
    Task<IReadOnlyList<Car>> GetAll(CancellationToken cancellationToken);
    Task<IReadOnlyList<Car>> GetFiltered(CarFilter filter, CancellationToken cancellationToken);
    Task<Car?> GetById(Guid id, CancellationToken cancellationToken);
}
