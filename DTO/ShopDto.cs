using Entities;

namespace DTO;

public class ShopResDto: BaseDto<ShopResDto, Shop>
{
    public required string Title { get; set; }
    public required string Image { get; set; }
    public required string Description { get; set; }
    public int UserId { get; set; }
}