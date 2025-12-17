using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VelocityBoard.Application.DTOs.ProjectDto;
using VelocityBoard.Application.Interfaces;
using VelocityBoard.Core.Models;
using VelocityBoard.Infrastructure.Data;

namespace VelocityBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var projects = await _projectService.GetProjectsForUserAsync(userId);
            return Ok(projects);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var newProject = await _projectService.CreateProjectAsync(createProjectDto, userId);
            return CreatedAtAction(nameof(GetProject), new { id = newProject.Id }, newProject);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] CreateProjectDto updateProjectDto)
        {
            var success = await _projectService.UpdateProjectAsync(id, updateProjectDto);
            if (!success) return NotFound();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var success = await _projectService.DeleteProjectAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
