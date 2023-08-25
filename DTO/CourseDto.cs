using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class CourseDto : BaseDto<CourseDto, Course>
{
    [Required] [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Required] [Display(Name = "توضیحات")] public string Description { get; set; } = null!;
    [Required] [Display(Name = "قیمت")] public float Price { get; set; }
    [Required] [Display(Name = "کاربر")] public int UserId { get; set; }
    [Required] [Display(Name = "وضعیت")] public CourseStatus Status { get; set; }
}

public class CourseResDto : BaseDto<CourseResDto, Course>
{
    [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Display(Name = "توضیحات")] public string Description { get; set; } = null!;
    [Display(Name = "وضعیت")] public CourseStatus Status { get; set; }
    [Display(Name = "قیمت")] public float Price { get; set; }
    [Display(Name = "کاربر")] public int UserId { get; set; }
    [Display(Name = "نام کاربر")] public string? UserFullName { get; set; }
    [Display(Name = "تصویر")] public string? Image { get; set; }
    
}

