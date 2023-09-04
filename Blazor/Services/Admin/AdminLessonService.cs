using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;
using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Services.Admin;

public class AdminLessonService : AdminService<Lesson, LessonDto, LessonResDto>, IAdminLessonService
{
    public AdminLessonService(
        HttpClient httpClient,
        SweetAlertService swal,
        NavigationManager navigationManager,
        IJSRuntime jsRuntime) : base(
        httpClient,
        swal,
        navigationManager,
        jsRuntime)
    {
    }

    public async Task<CourseResDto?> GetByCourseId(int courseId)
    {
        try
        {
            var response = await HttpClient.GetAsync($"admin/Courses/{courseId}/Lessons");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<CourseResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}