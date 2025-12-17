using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs.ProjectDto;
using VelocityBoard.Application.DTOs.ProjectDtos;

namespace VelocityBoard.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, int userId);
        Task<bool> UpdateProjectAsync(int id, CreateProjectDto updateProjectDto);
        Task<bool> DeleteProjectAsync(int id);
        
        Task<IEnumerable<ProjectDto>> GetProjectsForUserAsync(int userId);
    }
}
