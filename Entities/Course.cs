using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Course: BaseEntity
{
    [Required] public string Title { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public float Price { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public CourseStatus Status { get; set; } = CourseStatus.OnPerforming;

    public User User { get; set; } = null!;
    public IEnumerable<Lesson>? Lessons { get; set; }
}

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasOne(course => course.User)
            .WithMany(user => user.Courses)
            .HasForeignKey(course => course.UserId)
            .IsRequired();
        builder.HasMany(course => course.Lessons)
            .WithOne(lesson => lesson.Course)
            .HasForeignKey(lesson => lesson.CourseId);

    }
}

public enum CourseStatus
{
    [Display(Name = "در حال برگزاری")] OnPerforming = 1,
    [Display(Name = "اتمام")] Completed = 2,
    [Display(Name = "غیرفعال")] Disable = 0,
}