using Astrum.Infrastructure.Shared;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Api.Controllers;

[Route("[controller]")]
public class AntiForgeryController : ApiBaseController
{
    private readonly IAntiforgery _antiforgery;

    public AntiForgeryController(IAntiforgery antiforgery, IHttpContextAccessor httpContextAccessor) : base()
    {
        _antiforgery = antiforgery;
    }

    // [HttpGet]
    // public IActionResult GetToken()
    // {
    //     _antiforgery.SetCookieTokenAndHeader(HttpContext);
    //     return Ok();
    // }
}