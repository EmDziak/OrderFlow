using OrderFlow.Console.Models;
using OrderFlow.Console.Services;

var p1 = new Product(1, "Karma dla psa", 120m, "Jedzenie");
var p2 = new Product(2, "Smycz automatyczna", 85m, "Akcesoria");
var p3 = new Product(3, "Drapak dla kota", 250m, "Zabawki");
var p4 = new Product(4, "Szampon dla szczeniąt", 45m, "Higiena");
var p5 = new Product(5, "Przysmaki dentystyczne", 15m, "Jedzenie");

var c1 = new Customer(1, "Katarzyna Skibidi", true, "Gdańsk");
var c2 = new Customer(2, "Piotr Łasy", false, "Poznań");
var c3 = new Customer(3, "Marta Skąpa", true, "Gdańsk");

var orders = new List<Order>
{
    new Order(1, c1, [new OrderItem(p1, 2), new OrderItem(p5, 10)], DateTime.Now, OrderStatus.New),
    new Order(2, c2, [new OrderItem(p2, 1)], DateTime.Now, OrderStatus.Validated),
    new Order(3, c3, [new OrderItem(p3, 1), new OrderItem(p4, 2)], DateTime.Now, OrderStatus.Processing),
    new Order(4, c1, [], DateTime.Now, OrderStatus.New), 
    new Order(5, c2, [new OrderItem(p1, 50)], DateTime.Now, OrderStatus.Cancelled)
};

var validator = new OrderValidator();
var rules = new List<ValidationRule> { OrderValidator.HasItems, OrderValidator.NotTooExpensive };
var lambdas = new List<Func<Order, bool>> { o => o.Items.Count > 0 };

foreach (var o in orders) validator.Validate(o, rules, lambdas);

Console.WriteLine("\n ZADNIE 3");

Predicate<Order> isVipOrder = o => o.Customer.IsVip;
var vipOrders = orders.FindAll(isVipOrder);

Action<Order> printOrder = o => Console.WriteLine($"Zamówienie {o.Id} dla {o.Customer.Name} na kwotę: {o.TotalAmount:C}");
vipOrders.ForEach(printOrder);

Func<Order, object> summaryFunc = o => new { o.Id, Buyer = o.Customer.Name };
var summaries = orders.Select(o => summaryFunc(o));

Console.WriteLine("\nZADANIE 4");

var cityGroup = from o in orders
                join c in new List<Customer> { c1, c2, c3 } on o.Customer.Id equals c.Id
                group o by c.City into g
                select new { City = g.Key, Count = g.Count() };

foreach (var g in cityGroup) Console.WriteLine($"Miasto: {g.City}, Liczba zamówień: {g.Count}");

var allProducts = orders.SelectMany(o => o.Items).Select(i => i.Product.Name).Distinct();
Console.WriteLine("Sprzedane produkty: " + string.Join(", ", allProducts));

var customerSpendings = (from c in new List<Customer> { c1, c2, c3 }
                         join o in orders on c.Id equals o.Customer.Id into userOrders
                         select new { c.Name, Total = userOrders.Sum(x => x.TotalAmount) });

foreach (var cs in customerSpendings) Console.WriteLine($"Klient: {cs.Name}, Łącznie wydał: {cs.Total:C}");

Console.WriteLine("\nNaciśnij dowlny klawisz, aby zakończyć.");
Console.ReadKey();