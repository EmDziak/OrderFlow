using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(OrderFlowContext db)
    {
        if (await db.Products.AnyAsync()) return;

        var products = new List<Product> {
            new() { Name = "Karma dla psa", Price = 120m, Category = "Jedzenie", Stock = 100 },
            new() { Name = "Smycz", Price = 85m, Category = "Akcesoria", Stock = 50 },
            new() { Name = "Drapak", Price = 250m, Category = "Zabawki", Stock = 10 },
            new() { Name = "Szampon", Price = 45m, Category = "Higiena", Stock = 30 },
            new() { Name = "Przysmaki", Price = 15m, Category = "Jedzenie", Stock = 200 }
        };

        var customers = new List<Customer> {
            new() { Name = "Katarzyna Skibidi", IsVip = true, City = "Gdańsk", Email = "kasia@o2.pl" },
            new() { Name = "Piotr Łasy", IsVip = false, City = "Poznań" },
            new() { Name = "Marta Skąpa", IsVip = true, City = "Gdańsk" }
        };

        db.Products.AddRange(products);
        db.Customers.AddRange(customers);
        await db.SaveChangesAsync();

        var orders = new List<Order> {
            new() {
                CustomerId = customers[0].Id,
                Status = OrderStatus.New,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem> { new() { ProductId = products[0].Id, Quantity = 2, UnitPrice = products[0].Price } }
            }
        };

        db.Orders.AddRange(orders);
        await db.SaveChangesAsync();
        System.Console.WriteLine("Baza danych została zasilona!");
    }
}