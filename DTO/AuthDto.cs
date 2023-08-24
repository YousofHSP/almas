using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class LoginDto: BaseDto<LoginDto, User> 
{
    [Required, Display(Name = "نام کاربری")]public string? UserName { get; set; }
    [Required, Display(Name = "رمز")]public string? Password { get; set; }
}