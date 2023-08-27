using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using DTO;

namespace Blazor.Services.Admin;

public class AdminLessonService: IAdminLessonService
{
    private readonly HttpClient _httpClient;

    public AdminLessonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<LessonResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("admin/Lessons");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<LessonResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LessonResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"admin/Lessons/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<LessonResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LessonResDto?> Create(LessonDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("admin/Lessons/", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<LessonResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LessonResDto?> Update(int id, LessonDto dto)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"admin/Lessons/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<LessonResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CourseResDto?> GetByCourseId(int courseId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"admin/Courses/{courseId}/Lessons");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LessonResDto?> UploadFile(int id, MultipartFormDataContent file)
    {
        try
        {
            var response = await _httpClient.PostAsync($"admin/Lessons/{id}/UploadFile", file);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<LessonResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}