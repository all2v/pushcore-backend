using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.IssueDTO;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Astrum.Infrastructure.Integrations.YouTrack.WebHookApiControllers
{
    [Route("WebHookIssues/[action]")]
    [AllowAnonymous]
    public class WebHookIssuesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogHttpService _logHttpService;
        private readonly ILogAdminService _logAdminService;

        public WebHookIssuesController(IMapper mapper, ILogHttpService logHttpService, ILogAdminService logAdminService)
        {
            _mapper = mapper;
            _logHttpService = logHttpService;
            _logAdminService = logAdminService;
        }

        [HttpPost]
        public async Task<IActionResult> IssueChangedYouTrack()
        {
            using var str = new StreamReader(Request.Body);
            var json = await str.ReadToEndAsync();

            try
            {
                var value = JsonConvert.DeserializeObject<IssueEvent>(json);
                _logAdminService.Log<IssueEvent>(json, value, "Данные перехвата изменений", ModuleAstrum.TrackerProject);
                var trackerIssue = _mapper.Map<TrackerIssue>(value);
                var baseUrl = Request.Host;
                var url = $"https://{baseUrl}/api/tracker-project/tracker-issue/synchroniseissues";
                var response = await HttpHelper.PostAsJsonAsync(url, 
                    trackerIssue);
                var message = response.IsSuccess ? "Синхронизация удалась" : response.Exception.Message;
                _logHttpService.Log<string>(trackerIssue, message, url, "Перехват изменений в задаче", TypeRequest.POST,
                    ModuleAstrum.TrackerProject);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.HttpResponseMessage.ReasonPhrase);
                }
                //var result = Redirect("");
                //var redirection = RedirectToAction("SynchroniseIssues", "MyProjects", trackerIssue);
            }
            catch (Exception e)
            {
                _logHttpService.Log(json, "Ошибка запроса", HttpContext.Request.Path, "Перехват изменений в задаче",
                    TypeRequest.POST,
                    ModuleAstrum.TrackerProject, ResultStatus.Error);
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
