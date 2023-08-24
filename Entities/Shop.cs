using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Shop: BaseEntity
{
    [Required] public required string Title { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public int UserId { get; set; }

    public required User User { get; set; }
}

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder
            .HasOne(shop => shop.User)
            .WithMany(user => user.Shops)
            .HasForeignKey(shop => shop.UserId)
            .IsRequired();
    }
}