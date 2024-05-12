using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.ArticleDTO;
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
    [Route("WebHookArticles/[action]")]
    [AllowAnonymous]
    public class WebHookArticlesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogHttpService _logHttpService;
        private readonly ILogAdminService _logAdminService;

        public WebHookArticlesController(IMapper mapper, ILogHttpService logHttpService, ILogAdminService logAdminService)
        {
            _mapper = mapper;
            _logHttpService = logHttpService;
            _logAdminService = logAdminService;
        }

        [HttpPost]
        public async Task<IActionResult> ArticleChangedYouTrack()
        {
            using var str = new StreamReader(Request.Body);
            var json = await str.ReadToEndAsync();

            try
            {
                var value = JsonConvert.DeserializeObject<ArticleEvent>(json);
                _logAdminService.Log<ArticleEvent>(json, value, "Данные перехвата изменений", ModuleAstrum.TrackerProject);
                var article = _mapper.Map<TrackerArticle>(value);
                var baseUrl = Request.Host;
                var url = $"https://{baseUrl}/api/tracker-project/tracker-article/synchronisearticles";
                var response = await HttpHelper.PostAsJsonAsync(url, article);
                var message = response.IsSuccess ? "Синхронизация удалась" : response.Exception.Message;
                _logHttpService.Log<string>(article, message, url, "Перехват изменений в статье", TypeRequest.POST,
                    ModuleAstrum.TrackerProject);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.HttpResponseMessage.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                _logHttpService.Log(json, "Ошибка запроса", HttpContext.Request.Path, "Перехват изменений в статье",
                    TypeRequest.POST,
                    ModuleAstrum.TrackerProject, ResultStatus.Error);
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
