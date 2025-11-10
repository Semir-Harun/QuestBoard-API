using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestBoard.Api.Mappings;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Application.DTOs.Notifications;
using QuestBoard.Application.Services;
using QuestBoard.Infrastructure.Auth;
using QuestBoard.Infrastructure.BackgroundJobs;
using QuestBoard.Infrastructure.Email;
using QuestBoard.Infrastructure.Files;
using QuestBoard.Infrastructure.Persistence;
using QuestBoard.Infrastructure.Persistence.Repositories;

namespace QuestBoard.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddScoped<AuthService>();
        services.AddScoped<ProjectService>();
        services.AddScoped<TaskService>();
        services.AddScoped<CommentService>();
        services.AddScoped<NotificationService>();
        return services;
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<QuestDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Default")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IFileResourceRepository, FileResourceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton(Channel.CreateUnbounded<EmailMessage>());
        services.AddSingleton<IEmailSender, SmtpEmailSender>();
        services.AddSingleton<IFileStorage, LocalFileStorage>();
        services.AddHostedService<EmailBackgroundService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        return services;
    }
}
