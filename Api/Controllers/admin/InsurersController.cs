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

[ApiVersion("1")]
public class InsurersController: CrudController<InsurerDto, InsurerResDto, Insurer>
{
    private readonly IRepository<Upload> _uploadRepository;
    private readonly IWebHostEnvironment _env;
    private readonly SiteSettings _settings;

    public InsurersController(IRepository<Insurer> repository, 
        IRepository<Upload> uploadRepository,
        IOptions<SiteSettings> settings,
        IWebHostEnvironment env,
        IMapper mapper) : base(repository, mapper)
    {
        _uploadRepository = uploadRepository;
        _env = env;
        _settings = settings.Value;
    }

    [HttpGet]
    public override async Task<ApiResult<List<InsurerResDto>>> Get(CancellationToken cancellationToken)
    {
        var insurers = await Repository.TableNoTracking
            .ProjectTo<InsurerResDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var images = await _uploadRepository.TableNoTracking
            .Where(u => u.Parent.Equals(Parent.Insurer))
            .ToListAsync(cancellationToken);
        
        foreach (var insurer in insurers)
        {
            insurer.Description = insurer.Description?.Length > 100
                ? insurer.Description[..100] + "..."
                : insurer.Description;
            var image = images.FirstOrDefault(i => i.ParentId.Equals(insurer.Id));
            if(image is null) continue;
            insurer.Image = image.GeneratePath(_settings.Url);
        }
        return Ok(insurers);
    }
    [HttpPost("{id}/UploadImage")]
    public async Task<ApiResult<InsurerResDto>> UploadImage(int id, IFormFile image, CancellationToken cancellationToken)
    {
        var model = await Repository.TableNoTracking.SingleOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        if (model is null) throw new NotFoundException("دوره پیدا نشد.");
        var path = Path.Combine(_env.WebRootPath, "uploads");
        var currentImage = await _uploadRepository.Table
            .Where(u => u.Parent.Equals(Parent.Insurer))
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
        
        var upload = await image.UploadImage(path, model.Id, Parent.Insurer, cancellationToken);
        await _uploadRepository.AddAsync(upload, cancellationToken);

        var resDto = InsurerResDto.FromEntity(model, Mapper);
        resDto.Image = Path.Combine(_settings.Url, "uploads", upload.StoredFileName);
        
        return Ok(resDto);
    }
    [HttpDelete("{id:int}")]
    public override async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
    {
        var upload = await _uploadRepository.Table
            .Where(u => u.ParentId.Equals(id))
            .Where(u => u.Parent.Equals(Parent.Insurer))
            .FirstOrDefaultAsync(cancellationToken);
        if (upload is not null)
        {
            var path = Path.Combine(_env.WebRootPath, "uploads", upload.StoredFileName);
            var image = new FileInfo(path);
            if(image.Exists)
                image.Delete();
        }
        return await base.Delete(id, cancellationToken);
    }
}