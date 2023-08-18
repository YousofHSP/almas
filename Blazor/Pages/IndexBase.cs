using System.Collections;
using Blazor.Services.Contracts;
using DTO;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public class IndexBase: ComponentBase
{
    [Inject]
    public IShopService ShopService { get; set; }
    public IEnumerable<ShopResDto>? Shops { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Shops = await ShopService.GetBestShops();
    }
}