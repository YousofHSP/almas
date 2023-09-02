using Api.Models;
using AutoMapper;
using Common.Exceptions;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebFramework.Api;

namespace Api.Controllers.admin;

[ApiVersion("1")]
public class UsersController: CrudController<UserDto, UserResDto, User>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;

    public UsersController(
        IRepository<User> repository, 
        UserManager<User> userManager,
        IJwtService jwtService,
        IMapper mapper) : base(repository, mapper)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }
    
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<ActionResult> Token([FromForm]TokenRequest tokenRequest, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(tokenRequest.username);
        if (user is null)
            throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.password);
        if(!isPasswordValid)
            throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

        await _userManager.UpdateSecurityStampAsync(user);
        var jwt = await _jwtService.GenerateAsync(user);
        return new JsonResult(jwt);

    }
}