using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs.TaskDtos;

namespace VelocityBoard.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId);
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<bool> UpdateTaskAsync(int id, CreateTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> UpdateTaskItemStatusAsync(int id, Core.Models.TaskItemStatus newStatus);
        Task<IEnumerable<TaskDto>> GetTasksForUserAsync(int userId);
    }
}
