﻿using System.ComponentModel.DataAnnotations;
using Common.Utilities;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;

public class Upload: BaseEntity
{
    [Required] public required UploadType Type { get; set; }
    [Required] public required string Source { get; set; }
    [Required] public required Parent Parent { get; set; }
    [Required] public required int ParentId { get; set; }
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
}