using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerClient.Models;
using TaskManagerClient.Services;

namespace TaskManagerClient.Pages;

public class EditModel : PageModel
{
    private readonly TaskApiClient _apiClient;

    public EditModel(TaskApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public TaskItem Task { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result = await _apiClient.GetAsync(id);

        if (!result.Success)
        {
            ErrorMessage = result.Error;
            return Page();
        }

        Task = result.Task!;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _apiClient.UpdateAsync(Task.Id, Task);

        if (!result.Success)
        {
            ErrorMessage = result.Error;
            return Page();
        }

        return RedirectToPage("/Index");
    }
}