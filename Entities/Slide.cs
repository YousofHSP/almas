using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Slide: BaseEntity
{
    [Required]public string Title { get; set; } = null!;
    [Required]public string? Description { get; set; }
}