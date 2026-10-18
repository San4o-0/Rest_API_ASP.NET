using Application.Cars.Filters;
using Domain.Cars;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface ICarQueries
{
    Task<IReadOnlyList<Car>> GetAll(CancellationToken cancellationToken);
    Task<IReadOnlyList<Car>> GetFiltered(CarFilter filter, CancellationToken cancellationToken);
    Task<Option<Car>> GetById(CarId id, CancellationToken cancellationToken);
}
