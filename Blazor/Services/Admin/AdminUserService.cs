using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;

namespace Blazor.Services.Admin;

public class AdminUserService: IAdminUserService
{
    private readonly HttpClient _httpClient;
    private readonly SweetAlertService _swal;

    public AdminUserService(HttpClient httpClient, SweetAlertService swal)
    {
        _httpClient = httpClient;
        _swal = swal;
    }
    public async Task<List<UserResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("Admin/Users");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<UserResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<UserResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Admin/Users/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<UserResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<UserResDto?> Create(UserDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Admin/Users", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<UserResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<UserResDto?> Update(int id, UserDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"Admin/Users/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<UserResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Admin/Users/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            if(response.StatusCode == HttpStatusCode.NoContent) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");

        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}