using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Core.Models;

namespace VelocityBoard.Application.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Core.Models.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; } = string.Empty;
    }
}
