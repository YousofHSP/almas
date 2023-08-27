using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class LessonDto: BaseDto<LessonDto, Lesson>
{
    [Required] [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Display(Name = "توضیحات")] public string? Description { get; set; }
    [Required] [Display(Name = "نوع")] public LessonType LessonType { get; set; }
    [Required][Display(Name = "دوره")] public int CourseId { get; set; }
    
}

public class LessonResDto : BaseDto<LessonResDto, Lesson>
{
    [Display(Name = "عنوان")] public string Title { get; set; } = null!;
    [Display(Name = "توضیحات")] public string? Description { get; set; }
    [Display(Name = "نوع")] public LessonType LessonType { get; set; }
    [Display(Name = "دوره")] public int CourseId { get; set; }
    [Display(Name = "عنوان دوره")] public string CourseTitle { get; set; } = null!;
    [Display(Name = "فایل")] public string? File { get; set; }
}