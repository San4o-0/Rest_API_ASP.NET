using Api.Dtos;
using Application.Cars.Commands;
using Application.Cars.Filters;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("cars")]
[ApiController]
public class CarsController(
    ISender sender,
    ICarQueries carQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CarDto>>> GetCars(CancellationToken cancellationToken)
    {
        var cars = await carQueries.GetAll(cancellationToken);
        return cars.Select(CarDto.FromDomainModel).ToList();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<CarDto>>> SearchCars(
        [FromQuery] CarFilterDto request,
        CancellationToken cancellationToken)
    {
        var filter = new CarFilter(request.MinPrice, request.MaxPrice, request.MinYear, request.MaxYear);
        var cars = await carQueries.GetFiltered(filter, cancellationToken);
        return cars.Select(CarDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CarDto>> GetCar(Guid id, CancellationToken cancellationToken)
    {
        var car = await carQueries.GetById(id, cancellationToken);
        if (car is null)
        {
            return NotFound();
        }

        return CarDto.FromDomainModel(car);
    }

    [HttpPost]
    public async Task<ActionResult<CarDto>> CreateCar(
        [FromBody] CreateCarDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCarCommand
        {
            Vin = request.Vin,
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            Price = request.Price,
            FuelType = request.FuelType
        };

        try
        {
            var car = await sender.Send(input, cancellationToken);
            var dto = CarDto.FromDomainModel(car);
            return CreatedAtAction(nameof(GetCar), new { id = dto.Id }, dto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CarDto>> UpdateCar(
        Guid id,
        [FromBody] UpdateCarDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCarCommand
        {
            CarId = id,
            Vin = request.Vin,
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            Price = request.Price,
            FuelType = request.FuelType,
            IsSold = request.IsSold
        };

        try
        {
            var car = await sender.Send(input, cancellationToken);
            if (car is null)
            {
                return NotFound();
            }

            return CarDto.FromDomainModel(car);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCar(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteCarCommand
        {
            CarId = id
        };

        var deleted = await sender.Send(input, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
