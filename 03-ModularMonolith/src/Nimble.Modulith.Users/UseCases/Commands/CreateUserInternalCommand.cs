using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Users.UseCases.Commands;

/// <summary>
/// Internal command for creating a user within the Users module.
/// </summary>
public record CreateUserInternalCommand(
    string Email,
    string Password
) : ICommand<Result<string>>;  // Returns the user ID
