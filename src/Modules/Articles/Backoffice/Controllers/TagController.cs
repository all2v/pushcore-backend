using Astrum.Account.Services;
using Astrum.Articles.Requests;
using Astrum.Articles.Services;
using Astrum.Articles.ViewModels;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Articles.Backoffice.Controllers
{
    [Area("Articles")]
    [Route("[area]/[controller]")]
    public class TagController : ApiBaseController
    {
        private readonly ITagService _tagService;
        private readonly ILogHttpService _logger;

        public TagController(ITagService tagService, ILogHttpService logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        /// <summary>
        ///     Создать тэг
        /// </summary>
        [HttpPost]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(TagView), StatusCodes.Status200OK)]
        public async Task<Result<TagView>> Create([FromForm] TagRequest tagRequest)
        {
            var response = await _tagService.Create(tagRequest);
            _logger.Log(tagRequest, response, HttpContext, "Тег создан.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Articles);
            return response;
        }

        /// <summary>
        ///     Получить тэги по id категории
        /// </summary>
        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<TagView>), StatusCodes.Status200OK)]
        public async Task<Result<List<TagView>>> GetByCategoryId([FromRoute] Guid id)
        {
            return await _tagService.GetByCategoryId(id);
        }

        /// <summary>
        ///     Получить тэги по предикату
        /// </summary>
        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<TagView>), StatusCodes.Status200OK)]
        public async Task<Result<List<TagView>>> GetsByPredicate([FromQuery] int count = 10, [FromQuery] string predicate = null)
        {
            return await _tagService.GetByPredicate(count, predicate);
        }
    }
}
