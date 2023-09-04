using DTO;
using Entities.Common;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminService<TEntity, TDto, TResDto>
{
    Task<List<TResDto>?> Get();
    Task<TResDto?> Get(int id);
    Task<TResDto?> Create(TDto dto);
    Task<TResDto?> Update(int id, TDto dto);
    Task Delete(int id);
    Task<TResDto?> UploadFile(int id, MultipartFormDataContent file);


}