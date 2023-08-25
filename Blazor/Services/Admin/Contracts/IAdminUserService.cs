﻿using DTO;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminUserService
{
    Task<IEnumerable<UserResDto>?> Get();
    Task<UserResDto?> Get(int id);
    Task<UserResDto?> Create(UserDto dto);
    Task<UserResDto?> Update(int id, UserDto dto);
    Task Delete(int id);
}