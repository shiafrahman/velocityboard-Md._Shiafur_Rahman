using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace VelocityBoard.Web.Pages.Projects
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<ProjectDto> Projects { get; set; } = new();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            Projects = await GetProjectsFromApi();
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

            var response = await client.DeleteAsync($"/api/projects/{id}");

            if (response.IsSuccessStatusCode)
            {
                Projects = await GetProjectsFromApi();
            }

            return Page();
        }

        
        private async Task<List<ProjectDto>> GetProjectsFromApi()
        {
            
            var accessToken = User.FindFirstValue("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return new List<ProjectDto>();
            }

            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var response = await client.GetAsync("/api/projects");

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    var projects = await JsonSerializer.DeserializeAsync<List<ProjectDto>>(contentStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return projects ?? new List<ProjectDto>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    
                    return new List<ProjectDto>();
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error fetching projects: {ex.Message}");
            }

            return new List<ProjectDto>();
        }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
    }
}
