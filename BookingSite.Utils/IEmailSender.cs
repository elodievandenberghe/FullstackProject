using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace BookingSite.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendWithBrevo;


public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly BrevoApiConfig _brevoApiConfig;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<EmailSender> logger, IOptions<BrevoApiConfig> brevoApiConfig)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        _brevoApiConfig = brevoApiConfig.Value;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_brevoApiConfig.ApiKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(_brevoApiConfig.ApiKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new BrevoClient(apiKey);
        var response = await client.SendAsync(
                new Sender("Elodie Vandenberghe", "elodie.vandenberghe@outlook.com"),
                new List<Recipient> { new Recipient(toEmail, toEmail) },
                subject,
                message,
                false
            );
        if (response)
        {
            _logger.LogInformation("Success");
        }
        else
        {
            _logger.LogInformation("Failure");
        }
    }
}