namespace OrderFlow.Console.Models;

public enum OrderStatus { New, Validated, Processing, Completed, Cancelled }

public record Product(int Id, string Name, decimal Price, string Category);

public record OrderItem(Product Product, int Quantity)
{
    public decimal TotalPrice => Product.Price * Quantity;
}

public record Customer(int Id, string Name, bool IsVip, string City);

public record Order(int Id, Customer Customer, List<OrderItem> Items, DateTime OrderDate, OrderStatus Status)
{
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
}