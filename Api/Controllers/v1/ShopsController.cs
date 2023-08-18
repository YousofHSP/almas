using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;

namespace Api.Controllers.V1;

[ApiVersion("1")]
public class ShopsController: BaseController
{
    private readonly IRepository<Shop> _repository;
    private readonly IMapper _mapper;

    public ShopsController(IRepository<Shop> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResult<ShopResDto>> Get(int id)
    {
        var shop = await _repository.TableNoTracking
            .ProjectTo<ShopResDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(shop => shop.Id.Equals(id));
        if (shop is null) throw new NotFoundException("فروشگاه پیدا نشد");
        return shop!;
    }

    [HttpGet]
    public async Task<ApiResult<IEnumerable<ShopResDto>>> Get()
    {
        var shops = await _repository.TableNoTracking
            .ProjectTo<ShopResDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        if (shops.Count == 0) throw new AppException(ApiResultStatusCode.ListEmpty, "فروشگاهی یافت نشد");
        foreach (var shop in shops)
        {
            shop.Description = shop.Description[..100] + " ...";
        }
        return shops;
    }
}