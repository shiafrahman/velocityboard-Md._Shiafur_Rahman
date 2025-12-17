using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs.TaskDtos;
using VelocityBoard.Application.Interfaces;
using VelocityBoard.Core.Models;
using VelocityBoard.Infrastructure.Data;

namespace VelocityBoard.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId)
        {
            var tasks = await _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);
            return task == null ? null : _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = _mapper.Map<TaskItem>(createTaskDto);
            task.CreatedDate = DateTime.UtcNow;
            task.Status = Core.Models.TaskItemStatus.NotStarted;

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id);
        }

        public async Task<bool> UpdateTaskAsync(int id, CreateTaskDto updateTaskDto)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return false;

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.DueDate = updateTaskDto.DueDate;
            task.Priority = updateTaskDto.Priority;
            task.ProjectId = updateTaskDto.ProjectId;
            task.AssignedToUserId = updateTaskDto.AssignedToUserId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTaskItemStatusAsync(int id, Core.Models.TaskItemStatus newStatus)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return false;

            task.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<IEnumerable<TaskDto>> GetTasksForUserAsync(int userId)
        {
            var tasks = await _context.TaskItems
                .Where(t => t.AssignedToUserId == userId)
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }
    }
}
