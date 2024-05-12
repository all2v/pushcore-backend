using Astrum.Infrastructure.Shared;
using Astrum.Recruting.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Recruting.Backoffice.Controllers;

[Area("Kanban")]
[Route("[area]/[controller]")]
[AllowAnonymous]
public class KanbanBoardController : ApiBaseController
{
    private readonly IKanbanService _kanbanService;

    public KanbanBoardController(IKanbanService kanbanService)
    {
        _kanbanService = kanbanService;
    }

    [HttpPost("move")]
    [Produces(typeof(List<StageCardModel>))]
    public async Task<IActionResult> MoveCandidateCard(Guid cardId, Guid stageId)
    {
        return Ok(await _kanbanService.MoveCardToStage(cardId, stageId));
    }

    [HttpGet("board")]
    [Produces(typeof(BoardView))]
    public async Task<IActionResult> GetBoard(Guid vacancyId)
    {
        return Ok(await _kanbanService.GetBoard(vacancyId));
    }
    //TODO пока доски будут создаватся автоматически
    //
    // [HttpGet("board/createDefault")]
    // [Produces(typeof(BoardView))]
    // public async Task<IActionResult> CreateDefaultBoard() { return Ok(new BoardView()); }

    [HttpPost("event/createOrUpdate")]
    [Produces(typeof(List<CardEventModel>))]
    public async Task<IActionResult> CreateOrUpdateEvent(CardEventModel createEventRequest)
    {
        return Ok(_kanbanService.CreateOrUpdateCardEvent(createEventRequest));
    }

    [HttpPost("stage/createOrUpdate")]
    [Produces(typeof(BoardView))]
    public async Task<IActionResult> CreateOrUpdateStage(StageCreationModel createStageRequest)
    {
        return Ok(await _kanbanService.CreateOrUpdateStage(createStageRequest));
    }
    [HttpGet("stage/getAccountables")]
    [Produces(typeof(List<AccountableModel>))]
    public async Task<IActionResult> GetAccountables()
    {
        return Ok(await _kanbanService.GetAccountables());
    }

    [HttpPost("stage/remove")]
    [Produces(typeof(BoardView))]
    public async Task<IActionResult> RemoveStage(Guid stageId)
    {
        return Ok(await _kanbanService.DeleteStage(stageId));
    } 
    [HttpPost("stage/setCandidate")]
    public async Task<IActionResult> SetCandidateToStage(StageCardModel stageCardModel)
    {
        return Ok(await _kanbanService.SetCandidateToStage(stageCardModel));
    }

    [HttpGet("event/names")]
    [Produces(typeof(List<string>))]
    public async Task<IActionResult> GetEventNames() { return Ok(new List<string>()); }

    [HttpPatch("stage/comment")]
    public async Task<IActionResult> CreateComment(CommentModel commentModel)
    {
        await _kanbanService.CreateComment(commentModel);
        return Ok();
    }
}

