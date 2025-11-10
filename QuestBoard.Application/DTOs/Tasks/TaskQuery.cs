namespace QuestBoard.Application.DTOs.Tasks;

public sealed record TaskQuery(string? Status, string? Search, int Page = 1, int PageSize = 20);
