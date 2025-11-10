using QuestBoard.Application.DTOs.Notifications;

namespace QuestBoard.Application.Abstractions;

public interface IEmailSender
{
    Task QueueAsync(EmailMessage message, CancellationToken cancellationToken = default);
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
