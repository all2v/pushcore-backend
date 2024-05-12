using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Astrum.Infrastructure.Shared.Filters;

public class TagByAreaNameOperationFilter : IOperationFilter
{
    #region IOperationFilter Members

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            var areaName = controllerActionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(AreaAttribute), true)
                .Cast<AreaAttribute>().FirstOrDefault();
            if (areaName != null)
                operation.Tags = new List<OpenApiTag> {new() {Name = areaName.RouteValue}};
            else
                operation.Tags = new List<OpenApiTag> {new() {Name = controllerActionDescriptor.ControllerName}};
        }
    }

    #endregion
}
// AuthResponsesOperationFilter.cs