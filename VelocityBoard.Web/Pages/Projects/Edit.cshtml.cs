using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using VelocityBoard.Application.DTOs;

namespace VelocityBoard.Web.Pages.Projects
{
    [Authorize]
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
        public CreateProjectDto Project { get; set; } = new();
       


        public async Task<IActionResult> OnGetAsync()
        {
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToPage("/Account/Login");
            }

            if (Id.HasValue)
            {
                var projectDto = await GetProjectByIdFromApi(Id.Value, accessToken);
                if (projectDto == null)
                {
                    return NotFound();
                }

                Project.Name = projectDto.Name;
                Project.Description = projectDto.Description;
            }

            return Page();
        }

        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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
                response = await client.PutAsJsonAsync($"/api/projects/{Id}", Project);
            }
            else
            {
                response = await client.PostAsJsonAsync("/api/projects", Project);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Projects/Index");
            }

            return Page();
        }

        

        private async Task<ProjectDto?> GetProjectByIdFromApi(int id, string accessToken)
        {
            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"/api/projects/{id}");

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ProjectDto>(contentStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            
            return null;
        }
    }

    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
