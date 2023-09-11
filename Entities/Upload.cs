using System.ComponentModel.DataAnnotations;
using Common.Utilities;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Upload: BaseEntity
{
    [Required] public UploadType Type { get; set; }
    [Required] public Parent Parent { get; set; }
    [Required] public int ParentId { get; set; }
    public string FileName { get; set; } = null!;
    public string StoredFileName { get; set; } = null!;
    [Required] public string ContentType { get; set; } = null!;
}

public enum UploadType
{
    [Display(Name ="تصویر")] Image = 1,
    [Display(Name="ویدئو")] Video = 2,
    [Display(Name = "صدا")] Audio = 3, 
}

public enum Parent
{
    Shop,
    Lesson,
    Course,
    Insurer,
    Package,
    Slide,
}