using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using DTO;

namespace Blazor.Services.Admin;

public class AdminCourseService: IAdminCourseService
{
    private readonly HttpClient _httpClient;

    public AdminCourseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<CourseResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("admin/Courses");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<IEnumerable<CourseResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CourseResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"admin/Courses/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"admin/Courses/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
            if(response.StatusCode == HttpStatusCode.NoContent) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CourseResDto?> Create(CourseDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"admin/Courses", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CourseResDto?> Update(int id, CourseDto dto)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"admin/Courses/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CourseResDto?> UploadImage(int id, MultipartFormDataContent image)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"admin/Courses/{id}/UploadImage", image);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطای رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}