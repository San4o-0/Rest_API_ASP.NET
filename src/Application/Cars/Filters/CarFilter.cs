namespace Application.Cars.Filters;

public record CarFilter(
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinYear,
    int? MaxYear);
