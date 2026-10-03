using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerClient.Models;
using TaskManagerClient.Services;

namespace TaskManagerClient.Pages;

public class IndexModel : PageModel
{
    private readonly TaskApiClient _apiClient;

    public IndexModel(TaskApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<TaskItem> Tasks { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        var result = await _apiClient.GetAllAsync();

        if (!result.Success)
        {
            ErrorMessage = result.Error;
            return;
        }

        Tasks = result.Tasks;
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var result = await _apiClient.DeleteAsync(id);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Error;
        }
        else
        {
            TempData["SuccessMessage"] = "Задача успешно удалена.";
        }

        return RedirectToPage();
    }
}
