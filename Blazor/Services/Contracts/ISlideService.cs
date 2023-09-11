using DTO;

namespace Blazor.Services.Contracts;

public interface ISlideService
{
    Task<List<SlideResDto>?> Get();
}