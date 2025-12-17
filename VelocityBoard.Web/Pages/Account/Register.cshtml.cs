using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using VelocityBoard.Application.DTOs;
using VelocityBoard.Application.DTOs.UserDtos;

namespace VelocityBoard.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public CreateUserDto Input { get; set; } = new();

    public RegisterModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _httpClientFactory.CreateClient("VelocityBoardAPI");
        var response = await client.PostAsJsonAsync("/api/users/register", Input);

        if (response.IsSuccessStatusCode)
        {
            
            return RedirectToPage("/Account/Login");
        }
        else
        {
           
            var errorContent = await response.Content.ReadAsStringAsync();
            ErrorMessage = errorContent; 
            return Page();
        }
    }
}