using DTO;

namespace Blazor.Services.Admin.Contracts;

public interface IAdminLessonService
{
    Task<List<LessonResDto>?> Get();
    Task<LessonResDto?> Get(int id);
    Task<LessonResDto?> Create(LessonDto dto);
    Task<LessonResDto?> Update(int id, LessonDto dto);
    Task<CourseResDto?> GetByCourseId(int courseId);
    Task<LessonResDto?> UploadFile(int id, MultipartFormDataContent file);

}