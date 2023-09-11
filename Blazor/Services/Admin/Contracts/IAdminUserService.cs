using DTO;
using Entities;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminUserService: IAdminService<User, UserDto, UserResDto>
{
    Task<string> Login(LoginDto dto);
}