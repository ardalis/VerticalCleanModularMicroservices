using Ardalis.Result;
using Mediator;
using Nimble.Modulith.Users.Contracts;
using Nimble.Modulith.Users.UseCases.Commands;

namespace Nimble.Modulith.Users.Integrations;

/// <summary>
/// Integration handler that translates external CreateUserCommand (from Contracts)
/// to internal CreateUserInternalCommand for processing by the Users module.
/// </summary>
public class CreateUserCommandHandler(IMediator mediator)
    : ICommandHandler<CreateUserCommand, Result<string>>
{
    public async ValueTask<Result<string>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Translate external command to internal command
        var internalCommand = new CreateUserInternalCommand(
            command.Email,
            command.Password
        );

        // Delegate to internal handler
        return await mediator.Send(internalCommand, cancellationToken);
    }
}
