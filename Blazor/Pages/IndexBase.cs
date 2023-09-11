using Blazor.Services.Admin.Contracts;
using Blazor.Services.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public class IndexBase : ComponentBase
{
    [Inject] public IShopService ShopService { get; set; } = null!;
    [Inject] public IInsurerService InsurerService { get; set; } = null!;
    [Inject] public IPackageService PackageService { get; set; } = null!;
    protected IEnumerable<ShopResDto>? Shops { get; private set; }
    protected List<InsurerResDto>? Insurers { get; set; }
    protected List<PackageResDto>? Packages { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Shops = await ShopService.GetBestShops();
        Insurers = await InsurerService.GetTopInsurers();
        Packages = await PackageService.Get();
    }
}