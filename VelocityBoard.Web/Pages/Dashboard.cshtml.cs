using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace VelocityBoard.Web.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<string> Projects { get; set; } = new();

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("VelocityBoardAPI");

            
            var accessToken = await HttpContext.GetTokenAsync("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToPage("/Account/Login");
            }

            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            
            var response = await client.GetAsync("/api/projects");

            if (response.IsSuccessStatusCode)
            {
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                var projects = await JsonSerializer.DeserializeAsync<List<ProjectDto>>(contentStream);
                Projects = projects?.Select(p => p.Name).ToList() ?? new List<string>();
            }
            else
            {
                
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
