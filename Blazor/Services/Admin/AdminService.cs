using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;
using Entities.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pluralize.NET;

namespace Blazor.Services.Admin;

public class AdminService<TEntity, TDto,TResDto> : IAdminService<TEntity, TDto, TResDto>
{
    protected readonly HttpClient HttpClient;
    protected readonly SweetAlertService Swal;
    protected readonly NavigationManager NavigationManager;
    protected readonly IJSRuntime JsRuntime;
    protected readonly string Uri;

    public AdminService(
        HttpClient httpClient,
        SweetAlertService swal,
        NavigationManager navigationManager,
        IJSRuntime jsRuntime)
    {
        HttpClient = httpClient;
        Swal = swal;
        NavigationManager = navigationManager;
        JsRuntime = jsRuntime;
        var pluralizer = new Pluralizer();
        Uri = pluralizer.Pluralize(typeof(TEntity).Name);
    }
    
    public async Task _setAuthorizationHeader()
    {
        var token = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    public async Task<List<TResDto>?> Get()
    {
        try
        {
            await _setAuthorizationHeader();
            var response = await HttpClient.GetAsync($"admin/{Uri}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<TResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            NavigationManager.NavigateTo("/Login");
            throw;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<TResDto?> Get(int id)
    {
        try
        {
            await _setAuthorizationHeader();
            var response = await HttpClient.GetAsync($"admin/{Uri}/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            NavigationManager.NavigateTo("/Login");
            throw;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<TResDto?> Create(TDto dto)
    {
        try
        {
            await _setAuthorizationHeader();
            var response = await HttpClient.PostAsJsonAsync($"admin/{Uri}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            NavigationManager.NavigateTo("/Login");
            throw;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<TResDto?> Update(int id, TDto dto)
    {
        try
        {
            await _setAuthorizationHeader();
            var response = await HttpClient.PatchAsJsonAsync($"admin/{Uri}/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            NavigationManager.NavigateTo("/Login");
            throw;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            await _setAuthorizationHeader();
            var response = await HttpClient.DeleteAsync($"admin/{Uri}/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            if(response.StatusCode == HttpStatusCode.NoContent) throw new Exception(apiResult?.Message ?? "Unhandled Error");
        }
        catch (UnauthorizedAccessException e)
        {
            NavigationManager.NavigateTo("/Login");
            throw;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<TResDto?> UploadFile(int id, MultipartFormDataContent file)
    {
        try
        {
            await _setAuthorizationHeader();
            var response = await HttpClient.PostAsync($"admin/{Uri}/{id}/UploadFile", file);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            NavigationManager.NavigateTo("/Login");
            throw;
        }
        catch (Exception e)
        {
            await Swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}