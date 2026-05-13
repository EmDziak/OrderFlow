using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Models;
using OrderFlow.Console.Persistence;
using OrderFlow.Console.Services;

using var db = new OrderFlowContext();

await db.Database.MigrateAsync();
await DatabaseSeeder.SeedAsync(db);

var newOrder = new Order
{
    CustomerId = 1,
    Status = OrderStatus.New,
    OrderDate = DateTime.Now,
    Items = new List<OrderItem> { new() { ProductId = 2, Quantity = 1, UnitPrice = 85m } }
};
db.Orders.Add(newOrder);
await db.SaveChangesAsync();
Console.WriteLine($"Dodano zamówienie #{newOrder.Id}");

Console.WriteLine("\n--- ZAPYTANIA LINQ ---");

var vipHighValue = await db.Orders
    .Where(o => o.Customer.IsVip && o.Items.Sum(i => i.Quantity * i.UnitPrice) > 100)
    .Select(o => new { o.Id, o.Customer.Name })
    .ToListAsync();

var ranking = (await db.Customers
    .Select(c => new {
        c.Name,
        Total = c.Orders.SelectMany(o => o.Items).Sum(i => i.Quantity * i.UnitPrice)
    })
    .ToListAsync())
    .OrderByDescending(x => x.Total)
    .ToList();

var unsold = await db.Products
    .Where(p => !p.OrderItems.Any())
    .ToListAsync();

Console.WriteLine("\n--- TEST TRANSAKCJI ---");
await OrderProcessingService.ProcessOrderAsync(db, 1);
