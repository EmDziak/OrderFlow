using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public delegate bool ValidationRule(Order order, out string errorMessage);

public class OrderValidator
{
    public static bool HasItems(Order order, out string msg)
    {
        msg = order.Items.Any() ? "" : "Zamówienie jest puste!";
        return order.Items.Any();
    }

    public static bool NotTooExpensive(Order order, out string msg)
    {
        msg = order.TotalAmount < 10000 ? "" : "Kwota za wysoka na szybką walidację!";
        return order.TotalAmount < 10000;
    }

    public void Validate(Order order, List<ValidationRule> rules, List<Func<Order, bool>> lambdas)
    {
        System.Console.WriteLine($"\nWalidacja zamówinia nr {order.Id}");
        foreach (var rule in rules)
        {
            if (!rule(order, out var msg)) System.Console.WriteLine($"[BŁĄD DELEGATA]: {msg}");
        }
        foreach (var lambda in lambdas)
        {
            if (!lambda(order)) System.Console.WriteLine("[BŁĄD LAMBDA]: Reguła logiczna nie spełniona!");
        }
    }
}