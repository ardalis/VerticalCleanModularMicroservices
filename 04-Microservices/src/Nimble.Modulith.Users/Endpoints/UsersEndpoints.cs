using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Nimble.Modulith.Users.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this WebApplication app)
    {
        var users = app.MapGroup("/api/users")
            .WithTags("Users");

        // Get current user profile
        users.MapGet("/profile", async (ClaimsPrincipal user, UserManager<IdentityUser> userManager) =>
        {
            var currentUser = await userManager.GetUserAsync(user);
            if (currentUser is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(new { 
                Id = currentUser.Id, 
                Email = currentUser.Email,
                UserName = currentUser.UserName 
            });
        })
        .RequireAuthorization()
        .WithName("GetUserProfile")
        .WithSummary("Get current user profile");

        // Get all users (for testing/admin purposes)
        users.MapGet("/", async (UserManager<IdentityUser> userManager) =>
        {
            var allUsers = userManager.Users.Select(u => new { 
                Id = u.Id, 
                Email = u.Email, 
                UserName = u.UserName 
            }).ToList();
            
            return Results.Ok(allUsers);
        })
        .RequireAuthorization()
        .WithName("GetAllUsers")
        .WithSummary("Get all users");
    }
}