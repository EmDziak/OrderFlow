using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Console.Models;

public enum OrderStatus { New, Validated, Processing, Completed, Cancelled }

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "";
    public int Stock { get; set; } 

    public List<OrderItem> OrderItems { get; set; } = new();
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsVip { get; set; }
    public string City { get; set; } = "";
    public string? Email { get; set; }

    public List<Order> Orders { get; set; } = new();
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }

    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;
}