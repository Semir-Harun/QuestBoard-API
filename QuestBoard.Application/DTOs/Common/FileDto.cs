using System;

namespace QuestBoard.Application.DTOs.Common;

public sealed record FileDto(Guid Id, string FileName, string ContentType, string Path, long Length);
