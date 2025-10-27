namespace Nimble.Modulith.Email.Integrations;

public record EmailToSend(
  string To,
  string Subject,
  string Body,
  string? From = null);
