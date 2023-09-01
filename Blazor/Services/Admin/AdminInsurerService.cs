﻿using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;

namespace Blazor.Services.Admin;

public class AdminInsurerService: IAdminInsurerService
{
    private readonly HttpClient _httpClient;
    private readonly SweetAlertService _swal;

    public AdminInsurerService(HttpClient httpClient, SweetAlertService swal)
    {
        _httpClient = httpClient;
        _swal = swal;
    }
    public async Task<List<InsurerResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("admin/Insurers");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<InsurerResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<InsurerResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"admin/Insurers/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<InsurerResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<InsurerResDto?> Create(InsurerDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("admin/Insurers", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<InsurerResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<InsurerResDto?> Update(int id, InsurerDto dto)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"admin/Insurers/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<InsurerResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<InsurerResDto?> UploadImage(int id, MultipartFormDataContent image)
    {
        try
        {
            var response = await _httpClient.PostAsync($"admin/Insurers/{id}/UploadImage", image);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<InsurerResDto>>();
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
            var response = await _httpClient.DeleteAsync($"admin/Insurers/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}