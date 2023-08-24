using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Insurer: BaseEntity
{
    [Required] public required string Title { get; set; }
    [Required] public required int UserId { get; set; }
    public string? Description { get; set; }

    public User User { get; set; } = null!;
}

public class InsurerConfiguration : IEntityTypeConfiguration<Insurer>
{
    public void Configure(EntityTypeBuilder<Insurer> builder)
    {
        builder.HasOne(i => i.User)
            .WithMany(u => u.Insurers)
            .HasForeignKey(i => i.UserId)
            .IsRequired();
    }
}