using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using VelocityBoard.Application.DTOs;
using VelocityBoard.Core.Models;

namespace VelocityBoard.Web.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<TaskDto> Tasks { get; set; } = new();


        //public SelectList StatusOptions { get; set; } = new SelectList(Enum.GetValues(typeof(Core.Models.TaskItemStatus)));

        public SelectList StatusOptions { get; set; } = null!;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var statuses = Enum.GetValues(typeof(Core.Models.TaskItemStatus))
                       .Cast<Core.Models.TaskItemStatus>()
                       .Select(s => new { Value = s.ToString(), Text = s.ToString() })
                       .ToList();

            StatusOptions = new SelectList(statuses, "Value", "Text");
            Tasks = await GetTasksFromApi();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToPage("/Account/Login");
            }

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.DeleteAsync($"/api/tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Refresh the list after deletion
                Tasks = await GetTasksFromApi();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, Core.Models.TaskItemStatus status)
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var statusUpdateData = new { status }; // Create the JSON object for the PATCH request
            var jsonContent = JsonSerializer.Serialize(statusUpdateData);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json-patch+json");

            var response = await client.PatchAsync($"/api/tasks/{id}/status", content);

            PopulateStatusOptions();
            
            Tasks = await GetTasksFromApi();
            return Page();
        }

        private void PopulateStatusOptions()
        {
            var statuses = Enum.GetValues(typeof(Core.Models.TaskItemStatus))
                       .Cast<Core.Models.TaskItemStatus>()
                       .Select(s => new { Value = s.ToString(), Text = s.ToString() })
                       .ToList();

            StatusOptions = new SelectList(statuses, "Value", "Text");
        }

        private async Task<List<TaskDto>> GetTasksFromApi()
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return new List<TaskDto>();
            }

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var response = await client.GetAsync("/api/tasks");

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    var tasks = await JsonSerializer.DeserializeAsync<List<TaskDto>>(contentStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return tasks ?? new List<TaskDto>();
                }
            }
            catch (Exception ex)
            {
                // Log error if necessary
                Console.WriteLine($"Error fetching tasks: {ex.Message}");
            }

            return new List<TaskDto>();
        }
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

     public enum TaskItemStatus { NotStarted, InProgress, Completed }
}
