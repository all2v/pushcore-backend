using Astrum.Infrastructure.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Api.Controllers;

// [Authorize(Roles = "SuperAdmin")]
[Produces("application/json")]
[Route("[controller]")]
public class DevController : ApiBaseController
{
   
}