using DTO;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminInsurerService
{
    Task<List<InsurerResDto>?> Get();
    Task<InsurerResDto?> Get(int id);
    Task<InsurerResDto?> Create(InsurerDto dto);
    Task<InsurerResDto?> Update(int id,InsurerDto dto);
    Task<InsurerResDto?> UploadImage(int id,MultipartFormDataContent image);
    Task Delete(int id);
}