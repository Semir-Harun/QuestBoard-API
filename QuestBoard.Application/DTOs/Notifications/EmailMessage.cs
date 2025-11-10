namespace QuestBoard.Application.DTOs.Notifications;

public sealed record EmailMessage(Guid ToUserId, string Subject, string Body);
