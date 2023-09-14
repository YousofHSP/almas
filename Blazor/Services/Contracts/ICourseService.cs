using DTO;

namespace Blazor.Services.Contracts;

public interface ICourseService
{
    Task<List<CourseResDto>?> Get();
    Task<CourseResDto?> Get(int id);
    Task<List<LessonResDto>?> GetLessons(int id);
}