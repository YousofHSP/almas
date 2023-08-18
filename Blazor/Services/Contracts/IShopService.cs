using DTO;

namespace Blazor.Services.Contracts;

public interface IShopService
{
    Task<IEnumerable<ShopResDto>?> GetBestShops();
    Task<ShopResDto?> GetItem(int id);
    Task<IEnumerable<ShopResDto>?> Get();
}