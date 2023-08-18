using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;

namespace Api.Controllers.V1;

[ApiVersion("1")]
public class HomeController : BaseController
{
    private readonly IRepository<Shop> _shopRepository;
    private readonly IMapper _mapper;

    public HomeController(IRepository<Shop> shopRepository, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
    }
    [HttpGet("GetBestShops")]
    public async Task<ApiResult<List<ShopResDto>>> GetBestShops()
    {
        var shops = await _shopRepository.TableNoTracking
            .ProjectTo<ShopResDto>(_mapper.ConfigurationProvider)
            .Take(3)
            .ToListAsync();
        foreach (var shop in shops)
            shop.Description = shop.Description[..100] + " ...";
        return shops;
    }
    
}