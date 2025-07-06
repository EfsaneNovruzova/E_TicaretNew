namespace E_TicaretNew.Application.Abstracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(List<string> toEmails, string subject, string body);
}
