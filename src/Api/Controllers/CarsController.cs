using Api.Dtos;
using Application.Cars.Filters;
using Application.Cars.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("cars")]
[ApiController]
public class CarsController(ICarService carService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CarDto>>> GetCars(CancellationToken cancellationToken)
    {
        var cars = await carService.GetCars(cancellationToken);
        return cars.Select(CarDto.FromDomainModel).ToList();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<CarDto>>> SearchCars(
        [FromQuery] CarFilterDto request,
        CancellationToken cancellationToken)
    {
        var filter = new CarFilter(request.MinPrice, request.MaxPrice, request.MinYear, request.MaxYear);
        var cars = await carService.SearchCars(filter, cancellationToken);
        return cars.Select(CarDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CarDto>> GetCar(Guid id, CancellationToken cancellationToken)
    {
        var car = await carService.GetCar(id, cancellationToken);
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
        try
        {
            var car = await carService.Add(
                request.Vin,
                request.Brand,
                request.Model,
                request.Year,
                request.Price,
                request.FuelType,
                cancellationToken);

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
        try
        {
            var car = await carService.Update(
                id,
                request.Vin,
                request.Brand,
                request.Model,
                request.Year,
                request.Price,
                request.FuelType,
                request.IsSold,
                cancellationToken);

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
        var deleted = await carService.Delete(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
