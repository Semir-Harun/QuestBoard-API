using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Notifications;

namespace QuestBoard.Infrastructure.Email;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly Channel<EmailMessage> _channel;

    public SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger, Channel<EmailMessage> channel)
    {
        _configuration = configuration;
        _logger = logger;
        _channel = channel;
    }

    public async Task QueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(message, cancellationToken);
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var smtpSection = _configuration.GetSection("Smtp");
        var host = smtpSection["Host"];
    var port = int.TryParse(smtpSection["Port"], out var parsedPort) ? parsedPort : 25;
        var user = smtpSection["User"];
        var pass = smtpSection["Pass"];
        var from = smtpSection["From"] ?? "no-reply@questboard.local";

        if (string.IsNullOrWhiteSpace(host))
        {
            _logger.LogWarning("SMTP host not configured. Skipping email send to user {UserId}.", message.ToUserId);
            return;
        }

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = string.IsNullOrWhiteSpace(user) ? null : new NetworkCredential(user, pass)
        };

        using var mail = new MailMessage(from, from)
        {
            Subject = message.Subject,
            Body = message.Body
        };

        try
        {
            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to user {UserId}.", message.ToUserId);
        }
    }
}
