using QuestBoard.Domain.Entities;

namespace QuestBoard.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
