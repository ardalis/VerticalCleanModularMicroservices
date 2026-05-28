using System.Security.Claims;

namespace Nimble.Modulith.Customers.Infrastructure;

public interface ICustomerAuthorizationService
{
    bool IsAdminOrOwner(ClaimsPrincipal user, string customerEmail);
}

public class CustomerAuthorizationService : ICustomerAuthorizationService
{
    public bool IsAdminOrOwner(ClaimsPrincipal user, string customerEmail)
    {
        // Check if user is Admin
        if (user.IsInRole("Admin"))
        {
            return true;
        }

        // Check if user owns the customer record (matching email)
        var userEmail = user.FindFirst(ClaimTypes.Email)?.Value
                     ?? user.Identity?.Name
                     ?? string.Empty;

        return string.Equals(userEmail, customerEmail, StringComparison.OrdinalIgnoreCase);
    }
}
