using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Contracts;
using DTO;

namespace Blazor.Services;

public class ShopService: IShopService
{
    private readonly HttpClient _httpClient;

    public ShopService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<ShopResDto>?> GetBestShops()
    {
        try
        {
            var response = await _httpClient.GetAsync("Home/GetBestShops");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<IEnumerable<ShopResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Server Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ShopResDto?> GetItem(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Shops/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<ShopResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Server Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<ShopResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("Shops/");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<IEnumerable<ShopResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Server Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}