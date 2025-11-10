using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Common;

namespace QuestBoard.Infrastructure.Files;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _environment;

    public LocalFileStorage(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<FileSaveResult> SaveAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var webRoot = _environment.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
        var uploadRoot = Path.Combine(webRoot, "uploads");
        Directory.CreateDirectory(uploadRoot);

        var storedFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadRoot, storedFileName);

        await using (var output = File.Create(filePath))
        {
            await fileStream.CopyToAsync(output, cancellationToken);
        }

        var relativePath = Path.Combine("uploads", storedFileName).Replace("\\", "/");
        return new FileSaveResult(Guid.Empty, relativePath, fileName, contentType, new FileInfo(filePath).Length);
    }
}
