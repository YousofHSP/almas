using System.ComponentModel.DataAnnotations;
using Entities.Common;

namespace Entities;

public class Package: BaseEntity
{
    [Required] public string Title { get; set; } = null!;
    [Required] public float Price { get; set; }
    public string? Description { get; set; }
    
}