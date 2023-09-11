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

namespace Api.Controllers.admin;

[ApiVersion("1")]
public class SlidesController : CrudController<SlideDto, SlideResDto, Slide>
{
    private readonly IRepository<Upload> _uploadRepository;
    private readonly SiteSettings _siteSetting;
    private readonly IWebHostEnvironment _env;

    public SlidesController(
        IRepository<Slide> repository,
        IRepository<Upload> uploadRepository,
        IOptions<SiteSettings> siteSetting,
        IWebHostEnvironment env,
        IMapper mapper) : base(repository, mapper)
    {
        _uploadRepository = uploadRepository;
        _siteSetting = siteSetting.Value;
        _env = env;
    }

    [AllowAnonymous]
    [HttpGet]
    public override async Task<ApiResult<List<SlideResDto>>> Get(CancellationToken cancellationToken)
    {
        var slides = await Repository.TableNoTracking.ProjectTo<SlideResDto>(Mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        var images= await _uploadRepository.TableNoTracking.Where(i => i.Parent.Equals(Parent.Slide))
            .ToDictionaryAsync(i => i.ParentId, i => i, cancellationToken);


        foreach (var slide in slides)
        {
            if (images.TryGetValue(slide.Id, out var image))
                slide.Image = image.GeneratePath(_siteSetting.Url);
        }

        return Ok(slides);
    }


    [HttpPost("{id:int}/UploadFile")]
    public async Task<ApiResult<SlideResDto>> UploadImage(int id, IFormFile image, CancellationToken cancellationToken)
    {
        var model = await Repository.TableNoTracking
            .SingleOrDefaultAsync(slide => slide.Id.Equals(id), cancellationToken: cancellationToken);
        if (model is null) throw new NotFoundException("فروشگاه پیدا نشد");

        var path = Path.Combine(_env.WebRootPath, "uploads");
        var currentImage = await _uploadRepository.Table
            .Where(u => u.Parent.Equals(Parent.Slide))
            .Where(u => u.ParentId.Equals(model.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (currentImage is not null)
        {
            var currentPath = Path.Combine(path, currentImage.StoredFileName);
            var fileInfo = new FileInfo(currentPath);
            if (fileInfo.Exists)
                fileInfo.Delete();
            _uploadRepository.Entities.Remove(currentImage);
        }

        var upload = await image.UploadImage(path, model.Id, Parent.Slide, cancellationToken);
        await _uploadRepository.AddAsync(upload, cancellationToken);

        var resDto = SlideResDto.FromEntity(model, Mapper);
        resDto.Image = Path.Combine(_siteSetting.Url, "uploads", upload.StoredFileName);

        return Ok(resDto);
    }
}