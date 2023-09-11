using System.Collections;
using DTO;

namespace Blazor.Services.Contracts;

public interface IInsurerService
{
   Task<List<InsurerResDto>?> Get();
   Task<List<InsurerResDto>?> GetTopInsurers();
   Task<InsurerResDto?> Get(int id);
}