using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Notifications;

namespace QuestBoard.Application.Services;

public sealed class NotificationService
{
    private readonly IEmailSender _emailSender;

    public NotificationService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public Task QueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => _emailSender.QueueAsync(message, cancellationToken);
}
