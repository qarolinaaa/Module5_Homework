using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerClient.Models;
using TaskManagerClient.Services;

namespace TaskManagerClient.Pages;

public class DetailsModel : PageModel
{
    private readonly TaskApiClient _apiClient;

    public DetailsModel(TaskApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public TaskItem? Task { get; set; }

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result = await _apiClient.GetAsync(id);

        if (!result.Success)
        {
            ErrorMessage = result.Error;
            return Page();
        }

        Task = result.Task;

        return Page();
    }
}