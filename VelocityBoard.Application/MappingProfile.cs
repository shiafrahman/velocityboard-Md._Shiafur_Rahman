using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs.ProjectDto;
using VelocityBoard.Application.DTOs.ProjectDtos;
using VelocityBoard.Application.DTOs.TaskDtos;
using VelocityBoard.Application.DTOs.UserDtos;
using VelocityBoard.Core.Models;

namespace VelocityBoard.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser.UserName));

            CreateMap<CreateProjectDto, Project>();


            CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser.UserName));

            CreateMap<CreateTaskDto, TaskItem>();

            CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserDto>();
        }
    }
}
