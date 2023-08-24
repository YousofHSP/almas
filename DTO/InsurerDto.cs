using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO;

public class InsurerResDto: BaseDto<InsurerResDto, Insurer>
{
    [Required] public required string Title { get; set; }
    [Required] public required int UserId { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

}