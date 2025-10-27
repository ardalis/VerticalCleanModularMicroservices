using Ardalis.Result;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace Nimble.Modulith.Users.UseCases.Commands;

/// <summary>
/// Internal handler for creating users within the Users module.
/// </summary>
public class CreateUserInternalCommandHandler(UserManager<IdentityUser> userManager)
    : ICommandHandler<CreateUserInternalCommand, Result<string>>
{
    public async ValueTask<Result<string>> Handle(CreateUserInternalCommand command, CancellationToken cancellationToken)
    {
        var user = new IdentityUser
        {
            UserName = command.Email,
            Email = command.Email,
            EmailConfirmed = true // Auto-confirm for simplicity
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result<string>.Error($"Failed to create user: {errors}");
        }

        return Result<string>.Success(user.Id);
    }
}
