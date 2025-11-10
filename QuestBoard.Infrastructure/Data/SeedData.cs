using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestBoard.Domain.Entities;
using QuestBoard.Infrastructure.Persistence;
using DomainTaskStatus = QuestBoard.Domain.Enums.TaskStatus;

namespace QuestBoard.Infrastructure.Data;

public static class SeedData
{
    public static async Task EnsureSeedDataAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuestDbContext>();

        await context.Database.MigrateAsync(cancellationToken);

        var roleLookup = await EnsureRolesAsync(context, cancellationToken);
        var admin = await EnsureAdminAsync(context, roleLookup["Admin"], cancellationToken);
        await EnsureSampleProjectAsync(context, admin, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<Dictionary<string, Role>> EnsureRolesAsync(QuestDbContext context, CancellationToken cancellationToken)
    {
        var roles = new[] { "Admin", "Manager", "Member" };
        var lookup = new Dictionary<string, Role>(StringComparer.OrdinalIgnoreCase);

        foreach (var roleName in roles)
        {
            var role = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

            if (role is null)
            {
                role = new Role { Name = roleName };
                await context.Roles.AddAsync(role, cancellationToken);
            }

            lookup[roleName] = role;
        }

        return lookup;
    }

    private static async Task<User> EnsureAdminAsync(QuestDbContext context, Role adminRole, CancellationToken cancellationToken)
    {
        const string adminEmail = "admin@questboard.local";
        var admin = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == adminEmail, cancellationToken);

        if (admin is null)
        {
            admin = new User
            {
                Email = adminEmail,
                DisplayName = "QuestBoard Admin",
                PasswordHash = HashPassword("QuestBoard!123"),
                Role = adminRole
            };

            await context.Users.AddAsync(admin, cancellationToken);
        }
        else if (admin.RoleId != adminRole.Id)
        {
            admin.Role = adminRole;
        }

        return admin;
    }

    private static async Task EnsureSampleProjectAsync(QuestDbContext context, User admin, CancellationToken cancellationToken)
    {
        const string projectName = "QuestBoard Launch Plan";
        var existing = await context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Name == projectName, cancellationToken);

        if (existing is not null)
        {
            return;
        }

        var project = new Project
        {
            Name = projectName,
            Description = "Baseline backlog illustrating QuestBoard capabilities.",
            Owner = admin,
            Tasks = new List<TaskItem>
            {
                new()
                {
                    Title = "Enable CI pipeline",
                    Description = "Wire up GitHub Actions build and test workflow.",
                    Status = DomainTaskStatus.InProgress
                },
                new()
                {
                    Title = "Seed demo data",
                    Description = "Provide default admin account and sample project.",
                    Status = DomainTaskStatus.ToDo
                },
                new()
                {
                    Title = "Publish API documentation",
                    Description = "Share Swagger screenshot and diagrams in README.",
                    Status = DomainTaskStatus.ToDo
                }
            }
        };

        await context.Projects.AddAsync(project, cancellationToken);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = SHA256.HashData(Combine(salt, Encoding.UTF8.GetBytes(password)));
        return Convert.ToBase64String(Combine(salt, hash));
    }

    private static byte[] Combine(byte[] first, byte[] second)
    {
        var result = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, result, 0, first.Length);
        Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
        return result;
    }
}
