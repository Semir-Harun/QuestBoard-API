using System.Threading.Channels;
using QuestBoard.Application.DTOs.Notifications;

namespace QuestBoard.Infrastructure.BackgroundJobs;

public sealed class EmailQueue
{
    private readonly Channel<EmailMessage> _channel;

    public EmailQueue(Channel<EmailMessage> channel)
    {
        _channel = channel;
    }

    public ValueTask EnqueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => _channel.Writer.WriteAsync(message, cancellationToken);

    public IAsyncEnumerable<EmailMessage> ReadAllAsync(CancellationToken cancellationToken = default)
        => _channel.Reader.ReadAllAsync(cancellationToken);
}
