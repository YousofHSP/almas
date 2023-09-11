using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;

namespace Blazor.Services;

public class InsurerService: IInsurerService
{
    private readonly HttpClient _httpClient;
    private readonly SweetAlertService _swal;

    public InsurerService(
        HttpClient httpClient,
        SweetAlertService swal)
    {
        _httpClient = httpClient;
        _swal = swal;
    }

    public async Task<List<InsurerResDto>?> Get()
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
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }

    public async Task<List<InsurerResDto>?> GetTopInsurers()
    {
        try
        {
            var response = await _httpClient.GetAsync("home/GetTopInsurers");
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
            var response = await _httpClient.GetAsync($"Insurers/{id}");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<InsurerResDto>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "Server Error");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
    
}