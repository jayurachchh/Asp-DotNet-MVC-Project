namespace ProjectProject_Management2.Email_Services

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}
