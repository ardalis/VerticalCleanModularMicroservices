using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Users.Contracts;

public record CreateUserCommand(
    string Email,
    string Password
) : ICommand<Result<string>>;  // Returns the user ID

public record CreateUserResult(
    string UserId,
    string Email
);
