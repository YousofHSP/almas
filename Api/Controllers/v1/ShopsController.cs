using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Helpers;
using WebFramework.Api;

namespace Api.Controllers.V1;

[ApiVersion("1")]
[AllowAnonymous]
public class ShopsController: BaseController
{
    private readonly IRepository<Shop> _repository;
    private readonly SiteSettings _siteSettings;
    private readonly IRepository<Upload> _uploadRepository;
    private readonly IMapper _mapper;

    public ShopsController(IRepository<Shop> repository, IOptions<SiteSettings> siteSettings, IMapper mapper, IRepository<Upload> uploadRepository)
    {
        _repository = repository;
        _siteSettings = siteSettings.Value;
        _mapper = mapper;
        _uploadRepository = uploadRepository;
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResult<ShopResDto>> Get(int id, CancellationToken cancellationToken)
    {
        var shop = await _repository.TableNoTracking
            .ProjectTo<ShopResDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(shop => shop.Id.Equals(id), cancellationToken);
        if (shop is null) throw new NotFoundException("فروشگاه پیدا نشد");
        var image = await _uploadRepository.TableNoTracking
            .Where(upload => upload.Parent.Equals(Parent.Shop))
            .Where(upload => upload.ParentId.Equals(shop.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (image is not null)
            shop.Image = image.GeneratePath(_siteSettings.Url);
        
        return shop!;
    }

    [HttpGet]
    public async Task<ApiResult<IEnumerable<ShopResDto>>> Get(CancellationToken cancellationToken)
    {
        var shops = await _repository.TableNoTracking
            .ProjectTo<ShopResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        if (shops.Count == 0) throw new AppException(ApiResultStatusCode.ListEmpty, "فروشگاهی یافت نشد");
        var images = await _uploadRepository.TableNoTracking
            .Where(u => u.Parent.Equals(Parent.Shop))
            .ToListAsync(cancellationToken);
        foreach (var shop in shops)
        {
            shop.Description = shop.Description.Length > 60 ? shop.Description[..60] + " ..." : shop.Description;
            var image = images.FirstOrDefault(u => u.ParentId.Equals(shop.Id));
            if(image is null) continue;
            shop.Image = image.GeneratePath(_siteSettings.Url);
        }
        return shops;
    }
}