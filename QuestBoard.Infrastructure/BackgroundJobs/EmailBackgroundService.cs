using System;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Notifications;

namespace QuestBoard.Infrastructure.BackgroundJobs;

public sealed class EmailBackgroundService : BackgroundService
{
    private readonly Channel<EmailMessage> _channel;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailBackgroundService> _logger;

    public EmailBackgroundService(Channel<EmailMessage> channel, IEmailSender emailSender, ILogger<EmailBackgroundService> logger)
    {
        _channel = channel;
        _emailSender = emailSender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await _emailSender.SendAsync(message, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send queued email to user {UserId}", message.ToUserId);
            }
        }
    }
}
