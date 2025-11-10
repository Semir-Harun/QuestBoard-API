using Microsoft.AspNetCore.Authorization;

namespace QuestBoard.Application.Policies;

public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string ManagerOrAdmin = "ManagerOrAdmin";

    public static void Register(AuthorizationOptions options)
    {
        options.AddPolicy(AdminOnly, policy => policy.RequireRole("Admin"));
        options.AddPolicy(ManagerOrAdmin, policy => policy.RequireRole("Manager", "Admin"));
    }
}
