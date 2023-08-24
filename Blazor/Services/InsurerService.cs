using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Contracts;
using DTO;

namespace Blazor.Services;

public class InsurerService: IInsurerService
{
    private readonly HttpClient _httpClient;

    public InsurerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<InsurerResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("Insurers");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<InsurerResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Server Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<InsurerResDto?> Get(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Insurers/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<InsurerResDto>>();
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