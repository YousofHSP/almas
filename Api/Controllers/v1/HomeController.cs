using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
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
public class HomeController : BaseController
{
    private readonly IRepository<Shop> _shopRepository;
    private readonly IRepository<Upload> _uploadRepository;
    private readonly SiteSettings _siteSettings;
    private readonly IMapper _mapper;

    public HomeController(IRepository<Shop> shopRepository, IRepository<Upload> uploadRepository,
        IOptions<SiteSettings> siteSettings, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _uploadRepository = uploadRepository;
        _siteSettings = siteSettings.Value;
        _mapper = mapper;
    }

    [HttpGet("GetBestShops")]
    public async Task<ApiResult<List<ShopResDto>>> GetBestShops(CancellationToken cancellationToken)
    {
        var shops = await _shopRepository.TableNoTracking
            .ProjectTo<ShopResDto>(_mapper.ConfigurationProvider)
            .Take(3)
            .ToListAsync(cancellationToken);
        var images = await _uploadRepository.TableNoTracking
            .Where(u => u.Parent.Equals(Parent.Shop))
            .ToListAsync(cancellationToken);
        foreach (var shop in shops)
        {
            shop.Description = shop.Description.Length <= 100 ? shop.Description : shop.Description[..100] + " ...";
            var image = images.FirstOrDefault(u => u.ParentId.Equals(shop.Id));
            if(image is null) continue;
            shop.Image = image.GeneratePath(_siteSettings.Url);
        }

        return shops;
    }
}