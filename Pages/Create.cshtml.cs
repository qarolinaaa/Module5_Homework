using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerClient.Models;
using TaskManagerClient.Services;

namespace TaskManagerClient.Pages;

public class CreateModel : PageModel
{
    private readonly TaskApiClient _apiClient;

    public CreateModel(TaskApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public TaskItem Task { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _apiClient.CreateAsync(Task);

        if (!result.Success)
        {
            ErrorMessage = result.Error;
            return Page();
        }

        return RedirectToPage("/Index");
    }
}