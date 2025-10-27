namespace Nimble.Modulith.Email;

public record EmailMessage(
  string To,
  string Subject,
  string Body,
  string? From = null);
