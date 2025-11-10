using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Notifications;

namespace QuestBoard.Tests.TestUtilities;

public sealed class FakeEmailSender : IEmailSender
{
    public List<EmailMessage> SentMessages { get; } = new();

    public Task QueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        SentMessages.Add(message);
        return Task.CompletedTask;
    }

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        SentMessages.Add(message);
        return Task.CompletedTask;
    }
}
