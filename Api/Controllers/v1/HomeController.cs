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
    private readonly IRepository<Insurer> _insurerRepository;
    private readonly IRepository<Slide> _slideRepository;
    private readonly IRepository<Package> _packageRepository;
    private readonly SiteSettings _siteSettings;
    private readonly IMapper _mapper;

    public HomeController(
        IRepository<Shop> shopRepository, 
        IRepository<Upload> uploadRepository,
        IRepository<Insurer> insurerRepository,
        IRepository<Slide> slideRepository,
        IRepository<Package> packageRepository,
        IOptions<SiteSettings> siteSettings, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _uploadRepository = uploadRepository;
        _insurerRepository = insurerRepository;
        _slideRepository = slideRepository;
        _packageRepository = packageRepository;
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

    [HttpGet("GetTopInsurers")]
    public async Task<ApiResult<List<InsurerResDto>>> GetTopInsurers(CancellationToken cancellationToken)
    {
        var insurers = await _insurerRepository.TableNoTracking
            .Take(3)
            .ProjectTo<InsurerResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        var images = await _uploadRepository.TableNoTracking
            .Where(i => i.Parent.Equals(Parent.Insurer))
            .Where(i => insurers.Select(insurer => insurer.Id).Contains(i.ParentId))
            .ToDictionaryAsync(i => i.ParentId, i => i,cancellationToken);

        foreach (var insurer in insurers)
        {
            insurer.Description = insurer.Description?.Length > 100 ? insurer.Description[..100] : insurer.Description;
            if (images.TryGetValue(insurer.Id, out var image))
                insurer.Image = image.GeneratePath(_siteSettings.Url);
        }

        return Ok(insurers);
    }

    [HttpGet("GetSlides")]
    public async Task<ApiResult<List<SlideResDto>>> GetSlides(CancellationToken cancellationToken)
    {
        var slides = await _slideRepository.TableNoTracking
            .ProjectTo<SlideResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        var images = await _uploadRepository.TableNoTracking
            .Where(i => i.Parent.Equals(Parent.Slide))
            .ToListAsync(cancellationToken);

        foreach (var slide in slides)
        {
            var image = images.FirstOrDefault(i => i.ParentId.Equals(slide.Id));
            if(image is null) continue;
            slide.Image = image.GeneratePath(_siteSettings.Url);
        }

        return Ok(slides);
    }

    [HttpGet("GetPackages")]
    public async Task<ApiResult<List<PackageResDto>>> GetPackages(CancellationToken cancellationToken)
    {
        var packages = await _packageRepository.TableNoTracking
            .ProjectTo<PackageResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        var images = await _uploadRepository.TableNoTracking
            .Where(i => i.Parent.Equals(Parent.Package))
            .ToListAsync(cancellationToken);

        foreach (var package in packages)
        {
            package.Description = package.Description?.Length > 100
                ? package.Description[..100] + " ..."
                : package.Description;
            var image = images.FirstOrDefault(i => i.ParentId.Equals(package.Id));
            if(image is null) continue;
            package.Image = image.GeneratePath(_siteSettings.Url);
        }

        return Ok(packages);
    }
}