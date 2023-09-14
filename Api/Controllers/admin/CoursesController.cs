using AutoMapper;
using AutoMapper.QueryableExtensions;
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
public class CoursesController: CrudController<CourseDto, CourseResDto, Course>
{
    private readonly SiteSettings _settings;
    private readonly IRepository<Upload> _uploadRepository;
    private readonly IWebHostEnvironment _env;

    public CoursesController(IRepository<Course> repository,
        IRepository<Upload> uploadRepository,
        IMapper mapper,
        IOptions<SiteSettings> settings,
        IWebHostEnvironment env) : base(repository, mapper)
    {
        _settings = settings.Value;
        _uploadRepository = uploadRepository;
        _env = env;
    }

    [HttpPost("{id}/UploadFile")]
    public async Task<ApiResult<CourseResDto>> UploadImage(int id, IFormFile image, CancellationToken cancellationToken)
    {
        var model = await Repository.TableNoTracking.SingleOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        if (model is null) throw new NotFoundException("دوره پیدا نشد.");
        var path = Path.Combine(_env.WebRootPath, "uploads");
        var currentImage = await _uploadRepository.Table
            .Where(u => u.Parent.Equals(Parent.Course))
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
        
        var upload = await image.UploadImage(path, model.Id, Parent.Course, cancellationToken);
        await _uploadRepository.AddAsync(upload, cancellationToken);

        var resDto = CourseResDto.FromEntity(model, Mapper);
        resDto.Image = Path.Combine(_settings.Url, "uploads", upload.StoredFileName);
        
        return Ok(resDto);
    }

    [HttpGet("{id:int}/Lessons")]
    public async Task<ApiResult<CourseResDto>> GetCourseLessons(int id, CancellationToken cancellationToken)
    {
        var course = await Repository.TableNoTracking
            .Include(c => c.Lessons)
            .ProjectTo<CourseResDto>(Mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        if (course is null) throw new NotFoundException("دوره پیدا نشد.");

        var files = await _uploadRepository.TableNoTracking
            .Where(f => f.Parent.Equals(Parent.Lesson))
            .ToListAsync(cancellationToken);
        
        foreach (var lesson in course.Lessons ?? new List<LessonResDto>())
        {
            lesson.Description = lesson.Description?.Length > 60 ? lesson.Description[..60] : lesson.Description;
            var file = files.FirstOrDefault(f => f.ParentId.Equals(lesson.Id));
            if(file is null) continue;
            lesson.File = file.GeneratePath(_settings.Url);
        }
        return Ok(course);
    }
}