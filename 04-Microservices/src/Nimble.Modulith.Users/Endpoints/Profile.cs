using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Nimble.Modulith.Users.Endpoints;

public class ProfileResponse
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class Profile : EndpointWithoutRequest<ProfileResponse>
{
    public override void Configure()
    {
        Get("/profile");
        Summary(s => {
            s.Summary = "See your profile if logged in";
            s.Description = "See your profile";
        });
    }

    public override async Task<ProfileResponse> HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name;
        var email = User.FindFirstValue(ClaimTypes.Email);
        var response = new ProfileResponse
        {
            Id = userId ?? string.Empty,
            Email = email ?? string.Empty
        };

        await Send.OkAsync(response, ct);
        return response;
    }
}
