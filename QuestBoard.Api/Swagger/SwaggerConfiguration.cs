using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace QuestBoard.Api.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddQuestBoardSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "QuestBoard API",
                Version = "v1",
                Description = "Task and project management API"
            });
        });

        return services;
    }
}
