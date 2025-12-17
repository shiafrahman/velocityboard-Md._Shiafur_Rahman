using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Core.Models;

namespace VelocityBoard.Application.DTOs.TaskDtos
{
    public class UpdateTaskItemStatusDto
    {
        [Required]
        public TaskItemStatus Status { get; set; }
    }
}
