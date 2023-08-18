using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Lesson: BaseEntity
{
    
    [Required] public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required] public LessonType LessonType { get; set; }
    [Required] public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}

public class LessonConfiguration: IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasOne(lesson => lesson.Course)
            .WithMany(course => course.Lessons)
            .HasForeignKey(lesson => lesson.CourseId)
            .IsRequired();
    }
}

public enum LessonType
{
    [Display(Name = "فیلم")] Video = 1,
    [Display(Name = "صدا")] Audio = 2, 
}