using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Common;
using QuestBoard.Application.DTOs.Notifications;
using QuestBoard.Application.DTOs.Tasks;
using TaskStatusDomain = QuestBoard.Domain.Enums.TaskStatus;

namespace QuestBoard.Application.Services;

public sealed class TaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public TaskService(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<PagedResult<TaskDto>> ListAsync(TaskQuery query, CancellationToken cancellationToken = default)
    {
        var source = _unitOfWork.Tasks.Query();

    if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<TaskStatusDomain>(query.Status, true, out var status))
        {
            source = source.Where(t => t.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            source = source.Where(t => t.Title.Contains(query.Search));
        }

        var total = await source.CountAsync(cancellationToken);
        var items = await source
            .OrderByDescending(t => t.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<TaskDto>(items, total, query.Page, query.PageSize);
    }

    public async Task AssignAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId, cancellationToken)
                   ?? throw new KeyNotFoundException("Task not found.");

        await _unitOfWork.Tasks.AssignUserAsync(taskId, userId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var message = new EmailMessage(
            ToUserId: userId,
            Subject: $"Assigned to task: {task.Title}",
            Body: $"You have been assigned to '{task.Title}'."
        );

        await _emailSender.QueueAsync(message, cancellationToken);
    }
}
