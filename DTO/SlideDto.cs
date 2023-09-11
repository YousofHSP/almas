using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class SlideDto: BaseDto<SlideDto, Slide>
{
    [Required]public string Title { get; set; } = null!;
    public string? Description { get; set; }
}

public class SlideResDto : BaseDto<SlideResDto, Slide>
{
    [Display(Name = "عنوان")]public string Title { get; set; } = null!;
    [Display(Name = "تصویر")]public string? Image { get; set; }
    [Display(Name = "توضیحات")]public string? Description { get; set; }
}