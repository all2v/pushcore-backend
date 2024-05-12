using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Astrum.Infrastructure.Shared.Result.AspNetCore
{
    public class TranslateResultToActionResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if ((context.Result as ObjectResult)?.Value is not IResult result) return;

            if (context.Controller is not ControllerBase controller) return;

            context.Result = controller.ToActionResult(result);
        }
    }
}
