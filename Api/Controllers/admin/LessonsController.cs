using AutoMapper;
using Common;
using Common.Exceptions;
using Data.Contracts;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Helpers;
using WebFramework.Api;

namespace Api.Controllers.admin;
[ApiController]
[ApiVersion("1")]
public class LessonsController: CrudController<LessonDto, LessonResDto, Lesson>
{
    private readonly IRepository<Upload> _uploadRepository;
    private readonly IWebHostEnvironment _env;
    private readonly SiteSettings _settings;

    public LessonsController(
        IRepository<Lesson> repository,
        IRepository<Upload> uploadRepository,
        IWebHostEnvironment env,
        IOptions<SiteSettings> settings,
        IMapper mapper
        ) : base(repository, mapper)
    {
        _uploadRepository = uploadRepository;
        _env = env;
        _settings = settings.Value;
    }

    [HttpPost("{id:int}/UploadFile")]
    public async Task<ApiResult<LessonResDto>> UploadFile(int id, IFormFile file, CancellationToken cancellationToken)
    {
        var model = await Repository.TableNoTracking
            .SingleOrDefaultAsync(shop => shop.Id.Equals(id), cancellationToken: cancellationToken);
        if (model is null) throw new NotFoundException("فروشگاه پیدا نشد");

        var path = Path.Combine(_env.WebRootPath, "uploads");
        var currentImage = await _uploadRepository.Table
            .Where(u => u.Parent.Equals(Parent.Lesson))
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

        var fileType = model.LessonType switch
        {
            LessonType.Video => UploadType.Video,
            LessonType.Audio => UploadType.Audio,
            _ => throw new ArgumentOutOfRangeException()
        };
        var upload = await file.UploadAny(path, model.Id, Parent.Lesson, fileType, cancellationToken);
        await _uploadRepository.AddAsync(upload, cancellationToken);

        var resDto = LessonResDto.FromEntity(model, Mapper);
        resDto.File = Path.Combine(_settings.Url, "uploads", upload.StoredFileName);
        
        return Ok(resDto);
    }
}