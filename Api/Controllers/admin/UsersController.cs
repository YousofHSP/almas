using AutoMapper;
using Data.Contracts;
using DTO;
using Entities;
using WebFramework.Api;

namespace Api.Controllers.admin;

public class UsersController: CrudController<UserDto, UserResDto, User>
{
    public UsersController(IRepository<User> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}