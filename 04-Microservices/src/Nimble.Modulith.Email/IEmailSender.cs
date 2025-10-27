namespace Nimble.Modulith.Email;

public interface IEmailSender
{
  Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
