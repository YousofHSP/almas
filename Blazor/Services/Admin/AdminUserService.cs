using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazor.Services.Admin.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;
using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Services.Admin;

public class AdminUserService : AdminService<User, UserDto, UserResDto>, IAdminUserService
{

    public AdminUserService(
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
    public async Task<string> Login(LoginDto dto)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(dto.username, Encoding.UTF8), "username");
            formData.Add(new StringContent(dto.password, Encoding.UTF8), "password");
            formData.Add(new StringContent("password", Encoding.UTF8), "grant_type");

            var response = await HttpClient.PostAsync("Admin/Users/Token", formData);
            if (!response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
                throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            }

            var result = await response.Content.ReadFromJsonAsync<TokenResult>();
            return result?.access_token ?? string.Empty;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}

internal class TokenResult
{
    public string? access_token { get; set; }
    public string? refresh_token { get; set; }
    public string? token_type { get; set; }
    public int? expires_in { get; set; }
}