namespace Nimble.Modulith.Customers.Endpoints.Customers;

/// <summary>
/// Customer response for API (excludes internal tracking fields like UpdatedAt)
/// </summary>
public record CustomerResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    AddressResponse Address
);

public record AddressResponse(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);

public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    AddressRequest Address
);

public record AddressRequest(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    AddressRequest Address
);
