using Astrum.Infrastructure.Shared;
using Astrum.Recruting.Application.Service;
using Astrum.SharedLib.Application.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Recruting.Backoffice.Controllers;

[Area("Vacancy")]
[Route("[area]/[controller]")]
public class VacancyController : ApiBaseController
{
    private readonly IVacancyService _service;

    public VacancyController(IVacancyService service)
    {
        _service = service;
    }

    /// <summary>
    /// предполагаю что данный метод будет вызвыатся или периодически или во время перехода на страницу вакансий
    /// для обновления информации об активных вакансий
    /// </summary>
    /// <returns></returns>
    [HttpGet("vacancies")]
    [Produces(typeof(List<VacancyResponse>))]
    public async Task<IActionResult> GetVacancies(bool isActive)
    {
        return Ok(await _service.GetAllVacancies(isActive));
    }

    [HttpPost("create")]
    [Produces(typeof(CreateVacancyRequest))]
    public async Task<IActionResult> CreateOrUpdateVacancy(CreateVacancyRequest createVacancyRequest)
    {
        try
        {
            var userId = JwtManager.GetUserIdFromRequest(this.Request);
            if (!userId.HasValue)
                return BadRequest("Пользователь не найден");
            var response = await _service.CreateOrUpdateVacancy(userId.Value, createVacancyRequest);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // [HttpPost("create")]
    // [Produces(typeof(CreateVacancyRequest))]
    // public async Task<IActionResult> UpdateVacancy(CreateVacancyRequest createVacancyRequest)
    // {
    //     try
    //     {
    //         var userId = JwtManager.GetUserIdFromRequest(this.Request);
    //         if (!userId.HasValue)
    //             return BadRequest("Пользователь не найден");
    //         var response = await _service.CreateOrUpdateVacancy(userId.Value, createVacancyRequest);
    //         return Ok(response);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }

    [HttpPost("archive/{vacancyId:guid}")]
    public async Task<IActionResult> MoveVacancyToArchive(Guid vacancyId)
    {
        return Ok(_service.MoveVacancyToArchive(vacancyId));
    }

    /// <summary>
    /// получить подробную инфу по вакансии. Возможно сюда добавить инфу по рекрутинговой воронке
    /// </summary>
    /// <param name="vacancyId"></param>
    /// <returns></returns>
    [HttpGet("detail/{vacancyId:guid}")]
    [Produces(typeof(DetailVacancyResponse))]
    public async Task<IActionResult> GetDetailVacancy(Guid vacancyId)
    {
        return Ok(await _service.GetDetailVacancyInfo(vacancyId));
    }

    /// <summary>
    /// получить список откликов на вакансию
    /// </summary>
    /// <param name="vacancyId"></param>
    /// <returns></returns>
    [HttpGet("requests/{vacancyId:guid}")]
    [Produces(typeof(VacancyRequestResponse))]
    public async Task<IActionResult> GetVacancyRequests(Guid vacancyId)
    {
        return Ok(new List<VacancyRequestResponse>());
    }

    [HttpGet("vacancy/roles")]
    [Produces(typeof(List<IdNameModel<long>>))]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _service.GetRoles());
    }

    [HttpGet("vacancy/workTypes")]
    [Produces(typeof(List<IdNameModel<long>>))]
    public async Task<IActionResult> GetWorkTypes()
    {
        return Ok(await _service.GetWorkTypes());
    }

    [HttpGet("vacancy/vacancyTypes")]
    [Produces(typeof(List<IdNameModel<long>>))]
    public async Task<IActionResult> GetVacancyTypes()
    {
        return Ok(await _service.GetVacancyTypes());
    }
}