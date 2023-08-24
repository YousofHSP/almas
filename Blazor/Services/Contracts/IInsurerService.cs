using System.Collections;
using DTO;

namespace Blazor.Services.Contracts;

public interface IInsurerService
{
   Task<IEnumerable<InsurerResDto>?> Get();
   Task<InsurerResDto?> Get(int id);
}