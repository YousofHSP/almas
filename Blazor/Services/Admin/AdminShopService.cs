using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Admin.Contracts;
using DTO;

namespace Blazor.Services.Admin;

public class AdminShopService: IAdminShopService
{
    private readonly HttpClient _httpClient;

    public AdminShopService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ShopResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("admin/Shops");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<IEnumerable<ShopResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<ShopResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"admin/Shops/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<ShopResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
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
            var response = await _httpClient.DeleteAsync($"admin/Shops/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            if(response.StatusCode == HttpStatusCode.NoContent) throw new Exception(apiResult?.Message ?? "Unhandled Error");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ShopResDto?> Create(ShopDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("admin/Shops", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<ShopResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ShopResDto?> Update(int id, ShopDto dto)
    {
        
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"admin/Shops/{id}", dto);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<ShopResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ShopResDto?> UploadImage(int id, MultipartFormDataContent image)
    {
        try
        {
            var response = await _httpClient.PostAsync($"admin/Shops/{id}/UploadImage", image);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<ShopResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Unhandled Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}