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
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IHttpClientFactory httpClientFactory, ILogger<EditModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty]
        public CreateTaskDto Task { get; set; } = new();

        public SelectList Projects { get; set; } = new SelectList(new List<ProjectDto>());
        public SelectList Users { get; set; } = new SelectList(new List<UserDto>());

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateDropdowns();

            if (Id.HasValue)
            {
               
                var taskDto = await GetTaskByIdFromApi(Id.Value);
                if (taskDto == null)
                {
                    return NotFound();
                }
                
                Task.Title = taskDto.Title;
                Task.Description = taskDto.Description;
                Task.DueDate = taskDto.DueDate;
                Task.Priority = taskDto.Priority;
                Task.ProjectId = taskDto.ProjectId;
                Task.AssignedToUserId = taskDto.AssignedToUserId;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, repopulate dropdowns and return the page
                await PopulateDropdowns();
                return Page();
            }

            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToPage("/Account/Login");
            }

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;

            if (Id.HasValue)
            {
                response = await client.PutAsJsonAsync($"/api/tasks/{Id}", Task);
            }
            else
            {
                response = await client.PostAsJsonAsync("/api/tasks", Task);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                
                await PopulateDropdowns();
                ModelState.AddModelError(string.Empty, "An error occurred while saving the task. Please try again.");
                return Page();
            }
        }

        //private async Task PopulateDropdowns()
        //{
        //    var accessToken = User.FindFirstValue("AccessToken");
        //    if (string.IsNullOrEmpty(accessToken)) return;

        //    var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


        //    var projectsResponse = await client.GetAsync("/api/projects");
        //    if (projectsResponse.IsSuccessStatusCode)
        //    {
        //        var projectStream = await projectsResponse.Content.ReadAsStreamAsync();
        //        var projectList = await JsonSerializer.DeserializeAsync<List<ProjectDto>>(projectStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //        Projects = new SelectList(projectList ?? new List<ProjectDto>(), nameof(ProjectDto.Id), nameof(ProjectDto.Name));
        //    }


        //    var userList = new List<UserDto>
        //{
        //    new UserDto { Id = 1, UserName = "admin" }
        //};
        //    Users = new SelectList(userList, nameof(UserDto.Id), nameof(UserDto.UserName));
        //}

        // In Tasks/Edit.cshtml.cs, replace the PopulateDropdowns method
        private async Task PopulateDropdowns()
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return;

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Get Projects
            var projectsResponse = await client.GetAsync("/api/projects");
            if (projectsResponse.IsSuccessStatusCode)
            {
                var projectStream = await projectsResponse.Content.ReadAsStreamAsync();
                var projectList = await JsonSerializer.DeserializeAsync<List<ProjectDto>>(projectStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Projects = new SelectList(projectList ?? new List<ProjectDto>(), nameof(ProjectDto.Id), nameof(ProjectDto.Name));
            }

            // --- KEY CHANGE: Get all users from the API ---
            var usersResponse = await client.GetAsync("/api/users");
            if (usersResponse.IsSuccessStatusCode)
            {
                var userStream = await usersResponse.Content.ReadAsStreamAsync();
                var userList = await JsonSerializer.DeserializeAsync<List<UserDto>>(userStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Users = new SelectList(userList ?? new List<UserDto>(), nameof(UserDto.Id), nameof(UserDto.UserName));
            }
        }

        private async Task<TaskDto?> GetTaskByIdFromApi(int id)
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return null;

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"/api/tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TaskDto>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public int ProjectId { get; set; }
        public int AssignedToUserId { get; set; }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
