using System.Linq;
using System.Text;
using Astrum.SharedLib.Common.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Astrum.Api.Extensions;

public class DashedRoutingConvention : IControllerModelConvention
{
    #region IControllerModelConvention Members

    public void Apply(ControllerModel controller)
    {
        var hasRouteAttributes = controller.Selectors.Any(selector =>
            selector.AttributeRouteModel != null);
        if (hasRouteAttributes)
            // This controller manually defined some routes, so treat this 
            // as an override and not apply the convention here.
            return;

        foreach (var controllerAction in controller.Actions)
        {
            foreach (var selector in controllerAction.Selectors.Where(x => x.AttributeRouteModel == null))
            {
                var template = new StringBuilder();

                if (controllerAction.Controller.ControllerName != "Home")
                    template.Append(controller.ControllerName.PascalToKebabCase());

                if (controllerAction.ActionName != "Index")
                    template.Append("/" + controllerAction.ActionName.PascalToKebabCase());

                selector.AttributeRouteModel = new AttributeRouteModel
                {
                    Template = template.ToString()
                };
            }
        }
    }

    #endregion
}