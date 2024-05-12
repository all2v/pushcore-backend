using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Debts.Application.Models.CreateModels;
using Astrum.Debts.Application.Models.UpdateModels;
using Astrum.Debts.Application.Models.ViewModels;
using Astrum.Debts.Application.Services;
using Astrum.Debts.Domain.Aggregates;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Debts.Backoffice.Controllers
{
    [Route("[controller]")]
    public class DebtsController : ApiBaseController
    {
        private readonly IDebtsService _service;
        private readonly ILogHttpService _logger;
        public DebtsController(IDebtsService service, ILogHttpService logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("/api/debts/get-debts")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<DebtView>), StatusCodes.Status200OK)]
        public async Task<Result<List<DebtView>>> GetDebts()
        {
            var result = await _service.GetDebts();
            return result;
        }

        [HttpGet("/api/debts/get-debts-by-status")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<DebtView>), StatusCodes.Status200OK)]
        public async Task<Result<List<DebtView>>> GetDebtsByStatus([FromRoute] StatusDebt status)
        {
            var result = await _service.GetDebtsByStatus(status);
            return result;
        }

        [HttpGet("/api/debts/get-debts-by-debtor")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<DebtView>), StatusCodes.Status200OK)]
        public async Task<Result<List<DebtView>>> GetDebtsByDebtorId([FromRoute] Guid debtorId)
        {
            var result = await _service.GetDebtsByDebtorId(debtorId);
            return result;
        }

        [HttpGet("/api/debts/get-debts-by-borrower")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<DebtView>), StatusCodes.Status200OK)]
        public async Task<Result<List<DebtView>>> GetDebtsByBorrowerId([FromRoute] Guid borrowerId)
        {
            var result = await _service.GetDebtsByBorrowerId(borrowerId);
            return result;
        }

        [HttpGet("/api/debts/get-debt-by-id/{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(DebtView), StatusCodes.Status200OK)]
        public async Task<Result<DebtView>> GetDebtById([FromRoute]Guid id)
        {
            var result = await _service.GetDebtById(id);
            if (!result.IsSuccess)
            {
                _logger.Log(id, result, HttpContext, "Получена задолженность.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Debts);
            }
            return result;
        }

        [HttpGet("/api/debts/get-users-info")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<UserDebt>), StatusCodes.Status200OK)]
        public async Task<Result<List<UserDebt>>> GetUsers()
        {
            var result = await _service.GetUsers();
            return result;
        }

        [HttpPost("/api/debts/create-debt")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(DebtView), StatusCodes.Status200OK)]
        public async Task<Result<DebtView>> CreateDebt([FromBody] DebtCreateRequest debt)
        {
            var result = await _service.CreateDebt(debt);
            _logger.Log(debt, result, HttpContext, "Создана задолженность.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Debts);
            return result;
        }

        [HttpPut("/api/debts/update-debt/{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(DebtView), StatusCodes.Status200OK)]
        public async Task<Result<DebtView>> UpdateDebt([FromRoute]Guid id, [FromBody] DebtUpdateRequest debt)
        {
            var result = await _service.UpdateDebt(id, debt);
            _logger.Log(debt, result, HttpContext, "Обновлена задолженность.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Debts);
            return result;
        }

        [HttpDelete("/api/debts/delete-debt/{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(DebtView), StatusCodes.Status200OK)]
        public async Task<Result<DebtView>> DeleteDebt([FromRoute] Guid id)
        {
            var result = await _service.DeleteDebt(id);
            _logger.Log(id, result, HttpContext, "Удалена задолженность.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Debts);
            return result;
        }
    }
}
