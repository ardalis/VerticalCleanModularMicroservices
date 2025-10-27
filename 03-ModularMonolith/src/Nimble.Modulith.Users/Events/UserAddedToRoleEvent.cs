using Mediator;

namespace Nimble.Modulith.Users.Events;

public record UserAddedToRoleEvent(string UserId, string UserEmail, string RoleName) : INotification;
