using Blazor.Services.Contracts;
using DTO;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public class ShopBase : ComponentBase
{
    [Parameter] public int Id { get; set; }
    protected ShopResDto? Shop { get; set; }
    [Inject] private IShopService Service { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Shop = await Service.GetItem(Id);
    }
}