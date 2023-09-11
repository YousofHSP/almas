using DTO;

namespace Blazor.Services.Contracts;

public interface IPackageService
{
    Task<List<PackageResDto>?> Get();
}