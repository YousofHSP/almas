using DTO;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminCourseService
{
    Task<IEnumerable<CourseResDto>?> Get();
    Task<CourseResDto?> Get(int id);
    Task Delete(int id);
    Task<CourseResDto?> Create(CourseDto dto);
    Task<CourseResDto?> Update(int id, CourseDto dto);
    Task<CourseResDto?> UploadImage(int id, MultipartFormDataContent image);
}