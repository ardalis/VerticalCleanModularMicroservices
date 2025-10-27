using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Nimble.Modulith.Users.Endpoints;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? UserId { get; set; }
    public List<string>? Errors { get; set; }
}

public class Register(UserManager<IdentityUser> userManager) : 
    Endpoint<RegisterRequest, RegisterResponse>
{
    private readonly UserManager<IdentityUser> _userManager = userManager;

    public override void Configure()
    {
        Post("/register");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Register a new user";
            s.Description = "Creates a new user account";
        });
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var user = new IdentityUser 
        { 
            UserName = req.Email,
            Email = req.Email 
        };
        
        var result = await _userManager.CreateAsync(user, req.Password);
        
        if (result.Succeeded)
        {
            Response = new RegisterResponse
            {
                Success = true,
                Message = "User created successfully",
                UserId = user.Id
            };
            
            await Send.OkAsync(Response, ct);
        }
        else
        {
            AddError("User registration failed");
            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }

            await Send.ErrorsAsync(cancellation: ct);
        }
    }
}