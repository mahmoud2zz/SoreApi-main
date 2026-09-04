using System;
namespace CoffeeStoreApi.Services.Auth
{
	public interface IEmailService
	{
        Task  SendEmailAsync(string toEmail, string subject, string body);

    }
}

