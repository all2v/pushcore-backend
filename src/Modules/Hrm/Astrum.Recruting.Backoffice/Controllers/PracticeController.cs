using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Recruting.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Recruting.Backoffice.Controllers;

/// <summary>
///     Practice endpoint
/// </summary>
[Area("Practice")]
[Route("[area]/[controller]")]
[AllowAnonymous]
public class PracticeController : ApiBaseController
{
    private readonly IPracticeVacancyService _practiceVacancyService;

    public PracticeController(IPracticeVacancyService practiceVacancyService)
    {
        _practiceVacancyService = practiceVacancyService;
    }

    /// <summary>
    /// Метод для получения всех кандидатов на все направления практики
    /// </summary>
    /// <returns></returns>
    [HttpGet("candidates/all")]
    [Produces(typeof(List<Candidate>))]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCandidates()
    {
        return Ok(await _practiceVacancyService.GetAllCandidates());
    }

    /// <summary>
    /// Метод для получения направлений практики
    /// </summary>
    /// <param name="isActive">Определяет активные или архивные направления нужно возвращать</param>
    /// <returns></returns>
    [HttpGet("directions")]
    [Produces(typeof(List<PracticeDirection>))]
    [AllowAnonymous]
    public async Task<IActionResult> GetPracticeDirections(bool isActive)
    {
        return Ok(await _practiceVacancyService.GetPracticeDirections(isActive));
    }
    
    /// <summary>
    /// Метод для получения направлений практики
    /// </summary>
    /// <param name="practiceId">Ид практики</param>
    /// <returns></returns>
    [HttpGet("{practiceId:guid}")]
    [Produces(typeof(List<PracticeDirection>))]
    [AllowAnonymous]
    public async Task<IActionResult> GetPracticeDetail(Guid practiceId)
    {
        return Ok(await _practiceVacancyService.GetPracticeDetail(practiceId));
    }

    /// <summary>
    /// Метод для перемещения практики в архив
    /// </summary>
    /// <param name="practiceId"></param>
    /// <returns></returns>
    [HttpPost("directions/archive")]
    [Produces(typeof(List<PracticeDirection>))]
    [AllowAnonymous]
    public async Task<IActionResult> MovePracticeDirectionToArchive(Guid practiceId)
    {
        await _practiceVacancyService.MovePracticeDirectionToArchive(practiceId);
        return Ok();
    }

    /// <summary>
    /// Метод возвращает навыки практики
    /// </summary>
    /// <returns></returns>
    [HttpGet("skills")]
    [Produces(typeof(List<SkillModel>))]
    [AllowAnonymous]
    public async Task<IActionResult> GetPracticeSkills()
    {
        return Ok(await _practiceVacancyService.GetPracticeSkills());
    }

    /// <summary>
    /// Метод создает навыки практики
    /// </summary>
    /// <returns></returns>
    [HttpPost("skills/createOrUpdate")]
    [Produces(typeof(SkillModel))]
    [TranslateResultToActionResult]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePracticeSkills([FromBody] SkillModel skillModel)
    {
        return Ok(await _practiceVacancyService.CreateOrUpdateSkill(skillModel));
    }

    /// <summary>
    /// Метод создает навыки практики
    /// </summary>
    /// <returns></returns>
    [HttpPost("skills/delete")]
    [AllowAnonymous]
    public async Task<IActionResult> RemovePracticeSkills(Guid skillId)
    {
        await _practiceVacancyService.RemovePracticeSkills(skillId);
        return Ok();
    }

    /// <summary>
    /// Метод возвращает последний отклик кандидатов на направление 
    /// </summary>
    /// <param name="practiceId"></param>
    /// <returns></returns>
    [HttpGet("candidates")]
    [Produces(typeof(List<PracticeCandidate>))]
    [AllowAnonymous]
    public async Task<IActionResult> GetPracticeCandidatesByDirection(Guid practiceId)
    {
        return Ok(await _practiceVacancyService.GetPracticeCandidatesByPracticeId(practiceId));
    }

    /// <summary>
    /// Метод для создания/обновления данных по практике
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("createOrUpdate")]
    [Produces(typeof(PracticeCreateModel))]
    [AllowAnonymous]
    public async Task<IActionResult> UpdatePractice([FromBody] PracticeCreateModel model)
    {
        return Ok(await _practiceVacancyService.CreateOrUpdatePractice(model));
    }
    
    

    // [HttpPost("candidates")]
    // [Produces(typeof(List<PracticeCandidate>))]
    // public async Task<IActionResult> CreatePracticeCandidate([FromBody] PracticeCandidate candidate)
    // {
    //     return Ok(new PracticeCandidate());
    // }
}