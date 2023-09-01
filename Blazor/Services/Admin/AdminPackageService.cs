﻿using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using DTO;

namespace Blazor.Services.Admin;

public class AdminPackageService: IAdminPackageService
{
    private readonly HttpClient _httpClient;

    public AdminPackageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<PackageResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("admin/Packages");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<PackageResDto>?>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PackageResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"admin/Packages/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<PackageResDto>?>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PackageResDto?> Create(PackageDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("admin/Packages/", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<PackageResDto>?>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PackageResDto?> Update(int id, PackageDto dto)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"admin/Packages/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<PackageResDto>?>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult?.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PackageResDto?> UploadImage(int id, MultipartFormDataContent image)
    {
        try
        {
            var response = await _httpClient.PostAsync($"admin/Packages/{id}/UploadImage", image);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<PackageResDto>?>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult?.Data;
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
            var response = await _httpClient.DeleteAsync($"admin/Packages/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}