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

namespace Api.Controllers.V1;

[ApiVersion("1")]
public class CoursesController:BaseController
{
    private readonly IRepository<Course> _repository;
    private readonly IRepository<Upload> _uploadRepository;
    private readonly IMapper _mapper;
    private readonly SiteSettings _settings;

    public CoursesController(
        IRepository<Course> repository,
        IRepository<Upload> uploadRepository,
        IMapper mapper,
        IOptions<SiteSettings> settings)
    {
        _repository = repository;
        _uploadRepository = uploadRepository;
        _mapper = mapper;
        _settings = settings.Value;
    }
    
    [HttpGet]
    public async Task<ApiResult<List<CourseResDto>>> Get(CancellationToken cancellationToken)
    {
        var coursesTask = _repository.TableNoTracking
            .Include(c => c.Lessons)
            .ProjectTo<CourseResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        var imagesTask = _uploadRepository.TableNoTracking
            .Where(i => i.Parent.Equals(Parent.Course))
            .ToDictionaryAsync(i => i.ParentId, i => i,cancellationToken);
        await Task.WhenAll(coursesTask, imagesTask);

        var courses = await coursesTask;
        var images = await imagesTask;

        foreach (var course in courses)
        {
            course.Description = course.Description.Length > 100 ? course.Description[..100] : course.Description;
            if (images.TryGetValue(course.Id, out var image))
                course.Image = image.GeneratePath(_settings.Url);
        }

        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResult<CourseResDto>> Get(int id, CancellationToken cancellationToken)
    {
        var course = await _repository.TableNoTracking.Include(c => c.Lessons)
            .ProjectTo<CourseResDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        if (course is null) throw new NotFoundException("دوره پیدا نشد");
        var image = await _uploadRepository.TableNoTracking.Where(i => i.Parent.Equals(Parent.Course))
            .FirstOrDefaultAsync(i => i.ParentId.Equals(id), cancellationToken);
        if (image is not null)
            course.Image = image.GeneratePath(_settings.Url);
        return Ok(course);
    }
}