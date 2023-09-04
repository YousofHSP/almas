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
public class InsurersController : BaseController
{
    private readonly IRepository<Insurer> _repository;
    private readonly IRepository<Upload> _uploadRepository;
    private readonly SiteSettings _settings;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;

    public InsurersController(
        IRepository<Insurer> repository, 
        IRepository<Upload> uploadRepository,
        IOptions<SiteSettings> settings,
        IWebHostEnvironment env,
        IMapper mapper)
    {
        _repository = repository;
        _uploadRepository = uploadRepository;
        _settings = settings.Value;
        _env = env;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<InsurerResDto>>> Get(CancellationToken cancellationToken)
    {
        var insurers = await _repository.TableNoTracking
            .ProjectTo<InsurerResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        if (insurers.Count == 0) throw new AppException(ApiResultStatusCode.ListEmpty, "شرکت بیمه ای یافت");

        var images = await _uploadRepository.TableNoTracking
            .Where(i => i.Parent.Equals(Parent.Insurer))
            .ToListAsync(cancellationToken);
        
        foreach (var insurer in insurers)
        {
            insurer.Description = insurer.Description?.Length > 100 ? insurer.Description[..100] : insurer.Description;
            var image = images.FirstOrDefault(i => i.ParentId.Equals(insurer.Id));
            if(image is null) continue;
            insurer.Image = image.GeneratePath(_settings.Url);
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