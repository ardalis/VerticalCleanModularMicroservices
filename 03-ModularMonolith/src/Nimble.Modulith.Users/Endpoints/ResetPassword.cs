using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Nimble.Modulith.Email.Contracts;
using Nimble.Modulith.Users.Infrastructure;

namespace Nimble.Modulith.Users.Endpoints;

public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
}

public class ResetPassword(UserManager<IdentityUser> userManager, IMediator mediator) : 
    Endpoint<ResetPasswordRequest, ResetPasswordResponse>
{
    public override void Configure()
    {
        Post("/users/reset-password");
        AllowAnonymous(); // Allow anyone to request password reset
        Summary(s => {
            s.Summary = "Reset user password";
            s.Description = "Generates a new password and emails it to the user";
        });
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(req.Email);
        
        if (user == null)
        {
            // Don't reveal whether user exists or not for security
            Response = new ResetPasswordResponse
            {
                Success = true,
                Message = "If the email exists in our system, a password reset email has been sent."
            };
            return;
        }

        // Generate new password
        var newPassword = PasswordGenerator.GeneratePassword();

        // Remove old password and set new one
        var removeResult = await userManager.RemovePasswordAsync(user);
        if (!removeResult.Succeeded)
        {
            AddError("Failed to reset password");
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var addResult = await userManager.AddPasswordAsync(user, newPassword);
        if (!addResult.Succeeded)
        {
            AddError("Failed to set new password");
            foreach (var error in addResult.Errors)
            {
                AddError(error.Description);
            }
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        // Send email with new password
        var emailBody = $@"
Hello,

Your password has been reset successfully.

Your new temporary password is: {newPassword}

Please log in and change your password as soon as possible.

Best regards,
The Team
";

        var emailCommand = new SendEmailCommand(
            user.Email!,
            "Password Reset - New Temporary Password",
            emailBody
        );

        await mediator.Send(emailCommand, ct);

        Response = new ResetPasswordResponse
        {
            Success = true,
            Message = "If the email exists in our system, a password reset email has been sent."
        };
    }
}
