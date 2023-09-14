using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;

namespace Blazor.Services;

public class CourseService: ICourseService
{
    private readonly HttpClient _httpClient;
    private readonly SweetAlertService _swal;

    public CourseService(
        HttpClient httpClient,
        SweetAlertService swal)
    {
        _httpClient = httpClient;
        _swal = swal;
    }

    public async Task<List<CourseResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("Courses");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<CourseResDto>?>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<CourseResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Courses/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>?>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<List<LessonResDto>?> GetLessons(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Lessons/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<LessonResDto>>?>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}