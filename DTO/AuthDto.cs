using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class LoginDto: BaseDto<LoginDto, User> 
{
    [Required, Display(Name = "نام کاربری")]public string username { get; set; } = null!;
    [Required, Display(Name = "رمز")]public string password { get; set; } = null!;
    [Required]public string grant_type { get; set; } = null!;
}