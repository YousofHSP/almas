using Blazor.Services.Contracts;
using DTO;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public class ShopsBase: ComponentBase
{
    [Inject] private IShopService Service { get; set; } = null!;
    public IEnumerable<ShopResDto>? Shops { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Shops = await Service.Get();
    }
}