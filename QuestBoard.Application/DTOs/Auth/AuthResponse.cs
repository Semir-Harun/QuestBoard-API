using System;
using System.Collections.Generic;

namespace QuestBoard.Application.DTOs.Auth;

public sealed record AuthResponse(Guid UserId, string Email, string DisplayName, string Token, IReadOnlyCollection<string> Roles);
