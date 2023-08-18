using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Filters;

namespace WebFramework.Api
{

    [ApiController]
    [Authorize]
    [AllowAnonymous]
    [ApiResultFilter]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {

    }
}