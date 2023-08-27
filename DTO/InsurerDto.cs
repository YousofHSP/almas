using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class InsurerDto : BaseDto<InsurerDto, Insurer>
{
    [Required] [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Required] [Display(Name = "مدیر")] public int UserId { get; set; }
    [Display(Name = "توضیحات")] public string? Description { get; set; }
}

public class InsurerResDto : BaseDto<InsurerResDto, Insurer>
{
    [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Display(Name = "مدیر")] public int UserId { get; set; }
    [Display(Name = "مدیر")] public string UserFullName { get; set; }
    [Display(Name = "توضیحات")] public string? Description { get; set; }
    [Display(Name = "تصویر")] public string? Image { get; set; }
}