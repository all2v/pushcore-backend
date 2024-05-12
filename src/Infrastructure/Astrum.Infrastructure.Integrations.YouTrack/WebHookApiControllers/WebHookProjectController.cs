using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.IssueDTO;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Astrum.Infrastructure.Integrations.YouTrack.WebHookApiControllers
{
    //[Route("WebHookIssues/[action]")]
    //[AllowAnonymous]
    //public class WebHookProjectController : Controller
    //{
    //    private readonly IMapper _mapper;
    //    private readonly ILogHttpService _logHttpService;

    //    public WebHookProjectController(IMapper mapper, ILogHttpService logHttpService)
    //    {
    //        _mapper = mapper;
    //        _logHttpService = logHttpService;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> IssueChangedYouTrack()
    //    {
    //        using var str = new StreamReader(Request.Body);
    //        var json = await str.ReadToEndAsync();

    //        try
    //        {
    //            var value = JsonConvert.DeserializeObject<TrackerProjectEvent>(json);
    //            var trackerIssue = _mapper.Map<TrackerProject>(value);
    //            var baseUrl = Request.Host;
    //            var url = $"https://{baseUrl}/api/tracker-project/tracker-project/synchroniseprojects";
    //            var response = await HttpHelper.PostAsJsonAsync(url,
    //                trackerIssue);
    //            var message = response.IsSuccess ? "Синхронизация удалась" : response.Exception.Message;
    //            _logHttpService.Log<string>(trackerIssue, message, url, "Перехват изменений в проекте", TypeRequest.POST,
    //                ModuleAstrum.TrackerProject);
    //            if (!response.IsSuccess)
    //            {
    //                return BadRequest(response.HttpResponseMessage.ReasonPhrase);
    //            }
    //            //var result = Redirect("");
    //            //var redirection = RedirectToAction("SynchroniseIssues", "MyProjects", trackerIssue);
    //        }
    //        catch (Exception e)
    //        {
    //            var message = e +
    //                          "\n~---------------------------!YouTrack request!-----------------------------\n" +
    //                          json +
    //                          "\n~---------------------------!YouTrack end request!-----------------------------";
    //            _logHttpService.Log(null, message, HttpContext.Request.Path, "Перехват изменений в проекте",
    //                TypeRequest.POST,
    //                ModuleAstrum.TrackerProject, ResultStatus.Error, new[] { message });
    //            return BadRequest(e.Message);
    //        }

    //        return Ok();
    //    }
    //}
}
