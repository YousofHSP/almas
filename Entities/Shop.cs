using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Shop: BaseEntity
{
    [Required] public string Title { get; set; } = null!;
    [Required] public string Image { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public int UserId { get; set; }

    public User User { get; set; } = null!;
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