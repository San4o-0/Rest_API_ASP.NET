# CarDealership

Лабораторна робота №1. Web API на ASP.NET Core (.NET 10) з багатошаровою архітектурою.

Тема: **Автосалон**, сутність: **Car**.

## Сутність Car

| Поле        | Тип        | Опис                          |
|-------------|------------|-------------------------------|
| `Id`        | `Guid`     | Ідентифікатор                 |
| `Vin`       | `string`   | VIN-код, **унікальний**       |
| `Brand`     | `string`   | Марка                         |
| `Model`     | `string`   | Модель                        |
| `Year`      | `int`      | Рік випуску                   |
| `Price`     | `decimal`  | Ціна                          |
| `FuelType`  | `enum`     | `Petrol`, `Diesel`, `Gas`, `Hybrid`, `Electric` |
| `IsSold`    | `bool`     | Чи продано автомобіль         |
| `CreatedAt` | `DateTime` | Дата створення запису         |

Бізнес-правило: не можна створити або оновити автомобіль так, щоб з'явився дублікат VIN.

## Архітектура

```
Api ──► Infrastructure ──► Application ──► Domain
```
