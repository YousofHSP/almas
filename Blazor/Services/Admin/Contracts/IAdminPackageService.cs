using DTO;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminPackageService
{
    Task<List<PackageResDto>?> Get();
    Task<PackageResDto?> Get(int id);
    Task<PackageResDto?> Create(PackageDto dto);
    Task<PackageResDto?> Update(int id, PackageDto dto);
    Task<PackageResDto?> UploadImage(int id, MultipartFormDataContent image);
    Task Delete(int id);
    
}