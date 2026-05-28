namespace Nimble.Modulith.Users.Infrastructure;

public static class PasswordGenerator
{
    /// <summary>
    /// Generates a random password using a portion of a GUID.
    /// In production, consider more secure password generation with specific complexity requirements.
    /// </summary>
    /// <returns>A random password string</returns>
    public static string GeneratePassword()
    {
        // Take first 12 characters of a GUID (removes hyphens for simplicity)
        return Guid.NewGuid().ToString("N")[..12];
    }
}
