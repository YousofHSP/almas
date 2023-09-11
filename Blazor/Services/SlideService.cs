using System.Net;
using System.Net.Http.Json;
using Blazor.Services.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using DTO;

namespace Blazor.Services;

public class SlideService: ISlideService
{
    private readonly HttpClient _httpClient;
    private readonly SweetAlertService _swal;

    public SlideService(
        HttpClient httpClient,
        SweetAlertService swal)
    {
        _httpClient = httpClient;
        _swal = swal;
    }


    public async Task<List<SlideResDto>?> Get()
    {
        try
        {
            var response = await _httpClient.GetAsync("Home/GetSlides");
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<List<SlideResDto>>>();
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