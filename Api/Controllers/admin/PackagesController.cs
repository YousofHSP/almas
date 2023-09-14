using Api.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Helpers;
using Services;
using WebFramework.Api;

namespace Api.Controllers.admin;

[ApiVersion("1")]
public class PackagesController: CrudController<PackageDto, PackageResDto, Package>
{
    private readonly IRepository<Upload> _uploadRepository;
    private readonly SiteSettings _settings;
    private readonly IWebHostEnvironment _env;

    public PackagesController(
        IRepository<Package> repository, 
        IRepository<Upload> uploadRepository,
        IOptions<SiteSettings> settings,
        IWebHostEnvironment env,
        IMapper mapper) : base(repository, mapper)
    {
        _uploadRepository = uploadRepository;
        _settings = settings.Value;
        _env = env;
    }
 
    [HttpGet]
    public override async Task<ApiResult<List<PackageResDto>>> Get(CancellationToken cancellationToken)
    {
        var packages = await Repository.TableNoTracking.ProjectTo<PackageResDto>(Mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        var imageDictionary = await _uploadRepository.TableNoTracking
            .Where(i => i.Parent.Equals(Parent.Package))
            .ToDictionaryAsync(i => i.ParentId,i => i, cancellationToken);
    
        
        foreach (var package in packages)
        {
            package.Description = package.Description?.Length > 60 ? package.Description[..60] : package.Description;
            if (imageDictionary.TryGetValue(package.Id, out var image))
                package.Image = image.GeneratePath(_settings.Url);
        }
    
        return Ok(packages);
    }

    [HttpPost("{id}/UploadFile")]
    public async Task<ApiResult<PackageResDto>> UploadImage(int id, IFormFile image, CancellationToken cancellationToken)
    {
        var model = await Repository.TableNoTracking.SingleOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        if (model is null) throw new NotFoundException("دوره پیدا نشد.");
        var path = Path.Combine(_env.WebRootPath, "uploads");
        var currentImage = await _uploadRepository.Table
            .Where(u => u.Parent.Equals(Parent.Package))
            .Where(u => u.ParentId.Equals(model.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (currentImage is not null)
        {
            var currentPath = Path.Combine(path, currentImage.StoredFileName);
            var fileInfo = new FileInfo(currentPath);
            if(fileInfo.Exists)
                fileInfo.Delete();
            _uploadRepository.Entities.Remove(currentImage);
        }
        
        var upload = await image.UploadImage(path, model.Id, Parent.Package, cancellationToken);
        await _uploadRepository.AddAsync(upload, cancellationToken);

        var resDto = PackageResDto.FromEntity(model, Mapper);
        resDto.Image = Path.Combine(_settings.Url, "uploads", upload.StoredFileName);
        
        return Ok(resDto);
    }

    
}