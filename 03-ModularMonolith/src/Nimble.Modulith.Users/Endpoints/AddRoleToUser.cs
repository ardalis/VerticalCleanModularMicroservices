using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Nimble.Modulith.Users.Events;

namespace Nimble.Modulith.Users.Endpoints;

public class AddRoleToUserRequest
{
  public string RoleName { get; set; } = string.Empty;
}

public class AddRoleToUserResponse
{
  public bool Success { get; set; }
  public string Message { get; set; } = string.Empty;
}

public class AddRoleToUser : Endpoint<AddRoleToUserRequest, AddRoleToUserResponse>
{
  private readonly UserManager<IdentityUser> _userManager;
  private readonly IMediator _mediator;

  public AddRoleToUser(UserManager<IdentityUser> userManager, IMediator mediator)
  {
    _userManager = userManager;
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post("/users/{id}/roles");
    Roles("Admin"); // Only admins can assign roles to users
  }

  public override async Task HandleAsync(AddRoleToUserRequest req, CancellationToken ct)
  {
    var userId = Route<string>("id")!;
    
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      await Send.NotFoundAsync(ct);
      return;
    }

    // Normalize role name - if "admin" (any case), use "Admin", otherwise use as-is
    var normalizedRoleName = req.RoleName.Equals("admin", StringComparison.OrdinalIgnoreCase)
      ? "Admin"
      : req.RoleName;

    // Check if role exists
    if (normalizedRoleName != "Admin" && normalizedRoleName != "Customer")
    {
      AddError($"Role '{normalizedRoleName}' does not exist. Valid roles are: Admin, Customer");
      await Send.ErrorsAsync(cancellation: ct);
      return;
    }

    // Check if user already has the role
    if (await _userManager.IsInRoleAsync(user, normalizedRoleName))
    {
      AddError($"User is already in the '{normalizedRoleName}' role");
      await Send.ErrorsAsync(cancellation: ct);
      return;
    }

    // Add user to role
    var result = await _userManager.AddToRoleAsync(user, normalizedRoleName);
    
    if (!result.Succeeded)
    {
      AddError("Failed to add user to role");
      foreach (var error in result.Errors)
      {
        AddError(error.Description);
      }
      await Send.ErrorsAsync(cancellation: ct);
      return;
    }

    // Publish UserAddedToRoleEvent
    await _mediator.Publish(new UserAddedToRoleEvent(
      UserId: userId,
      UserEmail: user.Email!,
      RoleName: normalizedRoleName
    ), ct);

    Response = new AddRoleToUserResponse
    {
      Success = true,
      Message = $"User added to '{normalizedRoleName}' role successfully"
    };
  }
}
