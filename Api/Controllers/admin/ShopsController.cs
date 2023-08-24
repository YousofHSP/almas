using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Helpers;
using WebFramework.Api;

namespace Api.Controllers.admin;

[ApiController]
[ApiVersion("1")]
public class ShopsController : CrudController<ShopDto, ShopResDto, Shop>
{
    private readonly IRepository<Upload> _uploadRepository;
    private readonly SiteSettings _settings;
    private readonly IWebHostEnvironment _env;

    public ShopsController(IRepository<Shop> repository, IRepository<Upload> uploadRepository, IMapper mapper,
        IOptions<SiteSettings> settings,
        IWebHostEnvironment env) : base(repository, mapper)
    {
        _uploadRepository = uploadRepository;
        _settings = settings.Value;
        _env = env;
    }

    [HttpGet]
    public override async Task<ActionResult<List<ShopResDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await Repository.TableNoTracking
            .Include(shop => shop.User)
            .ProjectTo<ShopResDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        var images = await _uploadRepository.TableNoTracking
            .Where(upload => upload.Parent.Equals(Parent.Shop))
            .ToListAsync(cancellationToken);
        foreach (var item in result)
        {
            item.Description = item.Description.Length > 100 ? item.Description[..100] : item.Description;
            var image = images.FirstOrDefault(i => i.ParentId.Equals(item.Id));
            if (image is null) continue;
            var path = Path.Combine(_settings.Url, "uploads", image.StoredFileName);
            item.Image = path;
        }

        return Ok(result);
    }

    [HttpPost("{id:int}/UploadImage")]
    public async Task<ApiResult<ShopResDto>> UploadImage(int id, IFormFile image, CancellationToken cancellationToken)
    {
        var model = await Repository.TableNoTracking
            .Include(shop => shop.User)
            .SingleOrDefaultAsync(shop => shop.Id.Equals(id), cancellationToken: cancellationToken);
        if (model is null) throw new NotFoundException("فروشگاه پیدا نشد");

        var path = Path.Combine(_env.WebRootPath, "uploads");
        var currentImage = await _uploadRepository.Table
            .Where(u => u.Parent.Equals(Parent.Shop))
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
        
        var upload = await image.UploadImage(path, model.Id, Parent.Shop, cancellationToken);
        await _uploadRepository.AddAsync(upload, cancellationToken);

        var resDto = ShopResDto.FromEntity(model, Mapper);
        resDto.Image = Path.Combine(_settings.Url, "uploads", upload.StoredFileName);
        
        return Ok(resDto);
    }

    [HttpDelete("{id:int}")]
    public override async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
    {
        var upload = await _uploadRepository.Table
            .Where(u => u.ParentId.Equals(id))
            .Where(u => u.Parent.Equals(Parent.Shop))
            .FirstOrDefaultAsync(cancellationToken);
        if (upload is not null)
        {
            var path = Path.Combine(_env.WebRootPath, "uploads", upload.StoredFileName);
            var image = new FileInfo(path);
            if(image.Exists)
                image.Delete();
        }
        return await base.Delete(id, cancellationToken);
    }
}