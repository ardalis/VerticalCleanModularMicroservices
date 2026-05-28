namespace Nimble.Modulith.Customers.Domain.CustomerAggregate;

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string FullAddress => $"{Street}, {City}, {State} {PostalCode}, {Country}";
}
