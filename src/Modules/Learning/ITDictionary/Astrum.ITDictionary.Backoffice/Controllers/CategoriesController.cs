using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Services;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.ITDictionary.Controllers;

[Area("ITDictionary")]
[Route("[area]/[controller]")]
public class CategoriesController: ApiBaseController
{
    private readonly ILogHttpService _logger;
    private ICategoryService _categoryService;

    public CategoriesController(ILogHttpService logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }
    
    /// <summary>
    /// Получить категорию по id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(TermsCategoryView), 200)]
    public async Task<Result<TermsCategoryView>> GetById([FromRoute] Guid id)
    {
        var response = await _categoryService.GetByIdAsync(id);
        return response;
    }

    /// <summary>
    /// Создать категорию
    /// </summary>
    /// <param name="categoryRequest">Модель категории</param>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(TermsCategoryView), 200)]
    public async Task<Result<TermsCategoryView>> Create([FromBody] CategoryRequest categoryRequest)
    {
        var response = await _categoryService.CreateAsync(categoryRequest);

        return response;
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="id">Id категории</param>
    [HttpDelete("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    public async Task<Result> Delete([FromRoute] Guid id)
    {
        var response = await _categoryService.DeleteAsync(id);

        return response;
    }



    /// <summary>
    /// Получить список категорий
    /// </summary>
    [HttpGet("")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<TermsCategoryView>), 200)]
    public async Task<Result<List<TermsCategoryView>>> GetCategories()
    {
        var response = await _categoryService.GetAllAsync();
        return response;
    }
    
    /// <summary>
    /// Поиск категорий
    /// </summary>
    /// <param name="substring">Строка для поиска</param>
    [HttpGet("search")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(List<TermsCategoryView>), 200)]
    [ProducesDefaultResponseType(typeof(Result))]
    public async Task<Result<List<TermsCategoryView>>> SearchCategories([FromQuery] string substring)
    {
        var response = await _categoryService.SearchAsync(substring);
        return response;
    }
}