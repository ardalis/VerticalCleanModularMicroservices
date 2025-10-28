using Nimble.Modulith.Customers.Domain.Common;
using Nimble.Modulith.Customers.Infrastructure.Data;

namespace Nimble.Modulith.Customers.Domain.OrderAggregate;

// nsdepcop
//public class Foo(CustomersDbContext customers)
//{

//}

public class OrderItem : EntityBase
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}

