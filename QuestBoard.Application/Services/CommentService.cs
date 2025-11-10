using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Comments;

namespace QuestBoard.Application.Services;

public sealed class CommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommentDto> CreateAsync(CommentCreateDto dto, Guid authorId, CancellationToken cancellationToken = default)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(dto.TaskItemId, cancellationToken)
                   ?? throw new KeyNotFoundException("Task not found.");

        var comment = new Domain.Entities.Comment
        {
            TaskItemId = task.Id,
            Body = dto.Body,
            AuthorId = authorId,
        };

        await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<IReadOnlyList<CommentDto>> ListByTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Comments
            .Query()
            .Where(c => c.TaskItemId == taskId)
            .OrderBy(c => c.CreatedAt)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
