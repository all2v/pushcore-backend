using Astrum.CodeRev.Application.UserService.Services.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.CodeRev.Backoffice.UserService.Controllers;

[Area("CodeRev")]
[Route("[controller]")]
public class CardsController : ApiBaseController
{
    private readonly ICardService _cardService;
    private readonly ILogHttpService _logger;

    public CardsController(ICardService cardService, ILogHttpService logger)
    {
        this._cardService = cardService;
        _logger = logger;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<CardInfo>>))]
    [ProducesResponseType(typeof(List<CardInfo>), StatusCodes.Status200OK)]
    public async Task<Result<List<CardInfo>>> GetInterviewSolutions([FromQuery] int offset, [FromQuery] int limit)
    {
        return await _cardService.GetCards(offset, limit);
    }
}