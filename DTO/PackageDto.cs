using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class PackageDto: BaseDto<PackageDto, Package>
{
    [Required][Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Required][Display(Name = "قیمت")] public float Price { get; set; }
    [Display(Name = "توضیحات")]public string? Description { get; set; }
}

public class PackageResDto : BaseDto<PackageResDto, Package>
{
    [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Display(Name = "قیمت")] public float Price { get; set; }
    [Display(Name = "توضیحات")]public string? Description { get; set; }
    [Display(Name = "نصویر")]public string? Image { get; set; }
}