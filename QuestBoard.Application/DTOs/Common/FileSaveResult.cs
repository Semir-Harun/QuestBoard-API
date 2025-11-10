using System;

namespace QuestBoard.Application.DTOs.Common;

public sealed record FileSaveResult(Guid FileId, string Path, string FileName, string ContentType, long Length);
