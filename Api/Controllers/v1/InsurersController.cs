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
public class InsurersController : BaseController
{
    private readonly IRepository<Insurer> _repository;
    private readonly IMapper _mapper;

    public InsurersController(IRepository<Insurer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<InsurerResDto>>> Get()
    {
        var insurers = await _repository.TableNoTracking
            .ProjectTo<InsurerResDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        if (insurers.Count == 0) throw new AppException(ApiResultStatusCode.ListEmpty, "شرکت بیمه ای یافت");
        foreach (var insurer in insurers)
        {
            insurer.Description = string.IsNullOrEmpty(insurer.Description) ? "" : (insurer.Description[..100] + " ...");
        }

        return Ok(insurers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InsurerResDto>> Get(int id)
    {
        var insurer = await _repository.TableNoTracking.FirstOrDefaultAsync(i => i.Id.Equals(id));
        if (insurer is null) throw new NotFoundException("شرکت بیمه پیدا نشد");
        return Ok(insurer);
    }
}