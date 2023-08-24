using Blazor.Services.Contracts;
using DTO;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public class IndexBase: ComponentBase
{
    [Inject] public IShopService ShopService { get; set; } = null!;
    protected IEnumerable<ShopResDto>? Shops { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        Shops = await ShopService.GetBestShops();
    }
}