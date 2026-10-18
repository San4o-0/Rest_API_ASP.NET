using Application.Cars.Filters;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Cars;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CarRepository(ApplicationDbContext context) : ICarRepository, ICarQueries
{
    public async Task<IReadOnlyList<Car>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Cars.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Car>> GetFiltered(CarFilter filter, CancellationToken cancellationToken)
    {
        IQueryable<Car> query = context.Cars;

        if (filter.MinPrice is not null)
        {
            query = query.Where(x => x.Price >= filter.MinPrice);
        }

        if (filter.MaxPrice is not null)
        {
            query = query.Where(x => x.Price <= filter.MaxPrice);
        }

        if (filter.MinYear is not null)
        {
            query = query.Where(x => x.Year >= filter.MinYear);
        }

        if (filter.MaxYear is not null)
        {
            query = query.Where(x => x.Year <= filter.MaxYear);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Option<Car>> GetById(CarId id, CancellationToken cancellationToken)
    {
        var entity = await context.Cars
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<Car>.None;
    }

    public async Task<Option<Car>> GetByVin(string vin, CancellationToken cancellationToken)
    {
        var entity = await context.Cars
            .FirstOrDefaultAsync(x => x.Vin == vin, cancellationToken);

        return entity ?? Option<Car>.None;
    }

    public async Task<Car> Add(Car car, CancellationToken cancellationToken)
    {
        await context.Cars.AddAsync(car, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return car;
    }

    public async Task<Car> Update(Car car, CancellationToken cancellationToken)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync(cancellationToken);

        return car;
    }

    public async Task Delete(Car car, CancellationToken cancellationToken)
    {
        context.Cars.Remove(car);
        await context.SaveChangesAsync(cancellationToken);
    }
}
