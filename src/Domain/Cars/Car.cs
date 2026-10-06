namespace Domain.Cars;

public class Car
{
    public Guid Id { get; }
    public string Vin { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public decimal Price { get; private set; }
    public FuelType FuelType { get; private set; }
    public bool IsSold { get; private set; }
    public DateTime CreatedAt { get; }

    private Car(
        Guid id,
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType,
        bool isSold,
        DateTime createdAt)
    {
        Id = id;
        Vin = vin;
        Brand = brand;
        Model = model;
        Year = year;
        Price = price;
        FuelType = fuelType;
        IsSold = isSold;
        CreatedAt = createdAt;
    }

    public static Car New(
        Guid id,
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType)
        => new(id, vin, brand, model, year, price, fuelType, false, DateTime.UtcNow);

    public void UpdateDetails(
        string vin,
        string brand,
        string model,
        int year,
        decimal price,
        FuelType fuelType,
        bool isSold)
    {
        Vin = vin;
        Brand = brand;
        Model = model;
        Year = year;
        Price = price;
        FuelType = fuelType;
        IsSold = isSold;
    }
}
