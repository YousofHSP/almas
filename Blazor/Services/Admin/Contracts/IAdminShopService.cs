using DTO;

namespace Blazor.Services.Admin.Contracts;

internal interface IAdminShopService
{
    Task<IEnumerable<ShopResDto>?> Get();
    Task<ShopResDto?> Get(int id);
    Task<bool> Delete(int id);
    Task<ShopResDto?> Create(ShopDto dto);
    Task<ShopResDto?> Update(int id, ShopDto dto);
    Task<ShopResDto?> UploadImage(int id, MultipartFormDataContent image);
}