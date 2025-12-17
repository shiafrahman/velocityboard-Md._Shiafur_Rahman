using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Core.Models;

namespace VelocityBoard.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Email = "admin@velocityboard.com",
                    FirstName = "Admin",
                    LastName = "User",
                    PasswordHash = "$2y$10$ievgpt4SzPDI30dKyajspuWyeW2uQ6KvCsQZIa3koo1OT6g1Zm006"
                },

            new User { Id = 2, UserName = "john.doe", Email = "john.doe@example.com", FirstName = "John", LastName = "Doe", PasswordHash = "$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcflpP3OvI0Cz" }, // Same password for simplicity
        new User { Id = 3, UserName = "jane.smith", Email = "jane.smith@example.com", FirstName = "Jane", LastName = "Smith", PasswordHash = "$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcflpP3OvI0Cz" }
        );

            modelBuilder.Entity<Project>().HasData(
        new Project { Id = 1, Name = "Website Redesign", CreatedDate = DateTime.Now, CreatedByUserId = 1 },
        new Project { Id = 2, Name = "Mobile App Development", CreatedDate = DateTime.Now, CreatedByUserId = 2 },
        new Project { Id = 3, Name = "Marketing Campaign", CreatedDate = DateTime.Now, CreatedByUserId = 3 }



    );
            modelBuilder.Entity<TaskItem>().HasData(
        new TaskItem { Id = 1, Title = "Design Homepage", DueDate = DateTime.Now.AddDays(5), Status = TaskItemStatus.NotStarted, Priority = TaskPriority.High, ProjectId = 1, AssignedToUserId = 2, CreatedDate = DateTime.Now },
        new TaskItem { Id = 2, Title = "Setup Database", DueDate = DateTime.Now.AddDays(3), Status = TaskItemStatus.InProgress, Priority = TaskPriority.High, ProjectId = 1, AssignedToUserId = 1, CreatedDate = DateTime.Now },
        new TaskItem { Id = 3, Title = "Develop Login API", DueDate = DateTime.Now.AddDays(10), Status = TaskItemStatus.NotStarted, Priority = TaskPriority.Medium, ProjectId = 2, AssignedToUserId = 3, CreatedDate = DateTime.Now },
        new TaskItem { Id = 4, Title = "Create Ad Copy", DueDate = DateTime.Now.AddDays(7), Status = TaskItemStatus.NotStarted, Priority = TaskPriority.Low, ProjectId = 3, AssignedToUserId = 3, CreatedDate = DateTime.Now }
    );

            modelBuilder.Entity<Project>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.CreatedProjects)
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
