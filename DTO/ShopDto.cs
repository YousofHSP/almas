using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

[Serializable]
public class ShopResDto : BaseDto<ShopResDto, Shop>
{
    public required string Title { get; set; }
    public string Image { get; set; } = string.Empty;
    public required string Description { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = null!;
}

public class ShopDto : BaseDto<ShopDto, Shop>
{
    [Required(ErrorMessage = "فیلد عنوان اجباری است")]
    [Display(Name = "عنوان")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "فیلد توضیحات اجباری است")]
    [Display(Name = "توضیحات")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "فیلد کاربر اجباری است")]
    [Display(Name = "کاربر")]
    public int UserId { get; set; } = 1;
}