using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using VelocityBoard.Core.Models;

namespace VelocityBoard.Web.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<ProjectDto> Projects { get; set; } = new();
        public List<TaskDto> Tasks { get; set; } = new();

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Projects = await GetProjectsFromApi();
            Tasks = await GetTasksFromApi();
            return Page();
        }

        private async Task<List<ProjectDto>> GetProjectsFromApi()
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<ProjectDto>();

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("/api/projects");
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<ProjectDto>>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProjectDto>();
            }
            return new List<ProjectDto>();
        }

        private async Task<List<TaskDto>> GetTasksFromApi()
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<TaskDto>();

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("/api/tasks");
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<TaskDto>>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TaskDto>();
            }
            return new List<TaskDto>();
        }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string AssignedToUserName { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int AssignedToUserId { get; set; }
    }
}
