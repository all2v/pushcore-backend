using Astrum.Infrastructure.Shared;
using Astrum.Recruting.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Recruting.Backoffice.Controllers;

[Area("Candidates")]
[Route("[area]/[controller]")]
[AllowAnonymous]
public class CandidateController : ApiBaseController
{
    private readonly ICandidateService _candidateService;

    public CandidateController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpGet("candidates")]
    [Produces(typeof(List<CandidateInfoModel>))]
    public async Task<IActionResult> GetCandidatesList()
    {
        var result = await _candidateService.GetAllCandidates();
        return Ok(result);
    }

    [HttpGet("candidate/{candidateId:guid}")]
    public async Task<IActionResult> GetDetailCandidate(Guid candidateId)
    {
        return Ok(await _candidateService.GetDetailCandidate(candidateId));
    }

    [HttpPost("candidate/comment")]
    public async Task<IActionResult> CreateComment(CreateCommentRequest commentRequest)
    {
        await _candidateService.CreateComment(commentRequest);
        return Ok();
    }

    [HttpGet("candidate/comment")]
    [Produces(typeof(List<UserCommentModel>))]
    public async Task<IActionResult> GetCandidateComments(Guid candidateId)
    {
        return Ok(await _candidateService.GetCandidateComments(candidateId));
    }

    [HttpPost("candidate/create")]
    [Produces(typeof(CandidateInfoModel))]
    public async Task<IActionResult> CreateCandidate(CandidateCreateModel model)
    {
        var result = await _candidateService.CreateCandidate(model);
        return Ok(result);
    }
    
    [HttpPost("candidate/AddToPracticeVacancy")]
    [Produces(typeof(CandidateInfoModel))]
    public async Task<IActionResult> AddCandidateToPracticeVacancy(PracticeRequestCreate practiceCandidate)
    {
        var result = await _candidateService.AddToPracticeVacancy(practiceCandidate);
        return Ok(result);
    }
    
}

