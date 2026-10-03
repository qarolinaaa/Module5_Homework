using System.Net;
using System.Net.Http.Json;
using TaskManagerClient.Models;

namespace TaskManagerClient.Services;

public class TaskApiClient
{
    private readonly HttpClient _httpClient;

    public TaskApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, List<TaskItem> Tasks, string? Error)> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/tasks");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (false, new List<TaskItem>(), "Задачи не найдены.");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return (false, new List<TaskItem>(), "Ошибка сервера Web API.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return (
                    false,
                    new List<TaskItem>(),
                    $"Ошибка API: {(int)response.StatusCode}."
                );
            }

            var tasks = await response.Content
                .ReadFromJsonAsync<List<TaskItem>>();

            return (
                true,
                tasks ?? new List<TaskItem>(),
                null
            );
        }
        catch (HttpRequestException)
        {
            return (
                false,
                new List<TaskItem>(),
                "Не удалось подключиться к Web API."
            );
        }
    }

    public async Task<(bool Success, TaskItem? Task, string? Error)> GetAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/tasks/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (false, null, "Задача не найдена.");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return (false, null, "Ошибка сервера Web API.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return (
                    false,
                    null,
                    $"Ошибка API: {(int)response.StatusCode}."
                );
            }

            var task = await response.Content
                .ReadFromJsonAsync<TaskItem>();

            return (true, task, null);
        }
        catch (HttpRequestException)
        {
            return (false, null, "Не удалось подключиться к Web API.");
        }
    }

    public async Task<(bool Success, TaskItem? Task, string? Error)> CreateAsync(
        TaskItem task)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/tasks",
                task
            );

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return (false, null, "Некорректные данные задачи.");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return (false, null, "Ошибка сервера Web API.");
            }

            if (response.StatusCode != HttpStatusCode.Created)
            {
                return (
                    false,
                    null,
                    $"Ошибка API: {(int)response.StatusCode}."
                );
            }

            var createdTask = await response.Content
                .ReadFromJsonAsync<TaskItem>();

            return (true, createdTask, null);
        }
        catch (HttpRequestException)
        {
            return (false, null, "Не удалось подключиться к Web API.");
        }
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(
        int id,
        TaskItem task)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/tasks/{id}",
                task
            );

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return (false, "Некорректные данные задачи.");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (false, "Задача не найдена.");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return (false, "Ошибка сервера Web API.");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return (
                    false,
                    $"Ошибка API: {(int)response.StatusCode}."
                );
            }

            return (true, null);
        }
        catch (HttpRequestException)
        {
            return (false, "Не удалось подключиться к Web API.");
        }
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(
                $"api/tasks/{id}"
            );

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (false, "Задача не найдена.");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return (false, "Ошибка сервера Web API.");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return (
                    false,
                    $"Ошибка API: {(int)response.StatusCode}."
                );
            }

            return (true, null);
        }
        catch (HttpRequestException)
        {
            return (false, "Не удалось подключиться к Web API.");
        }
    }
}