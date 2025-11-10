using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Api.Extensions;
using QuestBoard.Api.Swagger;
using QuestBoard.Application.Policies;
using QuestBoard.Infrastructure.Auth;
using QuestBoard.Infrastructure.Data;
using QuestBoard.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddQuestBoardSwagger();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = JwtTokenService.TokenValidationParameters(builder.Configuration);
    });

builder.Services.AddAuthorization(AuthorizationPolicies.Register);

builder.Services.AddDirectoryBrowser();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

var app = builder.Build();

if (args.Contains("seed", StringComparer.OrdinalIgnoreCase))
{
    await SeedData.EnsureSeedDataAsync(app.Services);
    return;
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuestDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
