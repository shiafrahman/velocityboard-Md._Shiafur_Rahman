using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VelocityBoard.Core.Models
{
    public enum TaskItemStatus { NotStarted, InProgress, Completed }
    public enum TaskPriority { Low, Medium, High }
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedDate { get; set; }

        
        public int ProjectId { get; set; }
        public int AssignedToUserId { get; set; }

        
        public virtual Project Project { get; set; } = null!;
        public virtual User AssignedToUser { get; set; } = null!;
    }
}
