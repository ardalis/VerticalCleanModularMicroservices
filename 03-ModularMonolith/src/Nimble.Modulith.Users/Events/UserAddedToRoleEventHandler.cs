using Mediator;
using Nimble.Modulith.Email.Contracts;

namespace Nimble.Modulith.Users.Events;

public class UserAddedToRoleEventHandler : INotificationHandler<UserAddedToRoleEvent>
{
  private readonly IMediator _mediator;

  public UserAddedToRoleEventHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async ValueTask Handle(UserAddedToRoleEvent notification, CancellationToken cancellationToken)
  {
    var emailCommand = new SendEmailCommand(
      To: notification.UserEmail,
      Subject: $"You've been added to the {notification.RoleName} role",
      Body: $"Hello,\n\nYou have been added to the '{notification.RoleName}' role in the Nimble Modulith application.\n\nBest regards,\nThe Nimble Modulith Team"
    );

    await _mediator.Send(emailCommand, cancellationToken);
  }
}
