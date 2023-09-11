using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;

namespace Blazor.Services;

public class PackageService: IPackageService
{
    private readonly HttpClient _httpClient;
    private readonly SweetAlertService _swal;

    public PackageService(
        HttpClient httpClient,
        SweetAlertService swal)
    {
        _httpClient = httpClient;
        _swal = swal;
    }

    public async Task<List<PackageResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("Home/GetPackages");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<PackageResDto>>>();
            if (!response.IsSuccessStatusCode) throw new Exception(apiResult?.Message ?? "خطایی رخ داده است");
            return response.StatusCode == HttpStatusCode.NoContent ? default : apiResult!.Data;
        }
        catch (Exception e)
        {
            await _swal.FireAsync("خطا", e.Message, "error");
            throw;
        }
    }
}