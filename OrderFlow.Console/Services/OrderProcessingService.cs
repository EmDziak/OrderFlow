using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Models;
using OrderFlow.Console.Persistence;

namespace OrderFlow.Console.Services;

public static class OrderProcessingService
{
    public static async Task ProcessOrderAsync(OrderFlowContext db, int orderId)
    {
        using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            var order = await db.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Nie znaleziono zamówienia!");

            order.Status = OrderStatus.Processing;
            order.Notes = "W trakcie realizacji...";
            await db.SaveChangesAsync();
            foreach (var item in order.Items)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new Exception($"Brak towaru: {item.Product.Name}");

                item.Product.Stock -= item.Quantity;
            }

            order.Status = OrderStatus.Completed;
            await db.SaveChangesAsync();

            await transaction.CommitAsync();
            System.Console.WriteLine($"Sukces: Zamówienie #{orderId} zrealizowane.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            System.Console.WriteLine($"BŁĄD TRANSAKCJI: {ex.Message}");
        }
    }
}