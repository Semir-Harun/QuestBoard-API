using AutoMapper;
using QuestBoard.Application.DTOs.Comments;
using QuestBoard.Application.DTOs.Common;
using QuestBoard.Application.DTOs.Projects;
using QuestBoard.Application.DTOs.Tasks;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<TaskItem, TaskDto>().ReverseMap();
    CreateMap<Comment, CommentDto>().ReverseMap();
    CreateMap<FileResource, FileDto>().ReverseMap();
    }
}
