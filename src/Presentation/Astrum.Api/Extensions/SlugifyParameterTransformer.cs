using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Astrum.Api.Extensions;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    #region IOutboundParameterTransformer Members

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string? TransformOutbound(object? value)
    {
        // Slugify value
        return value == null
            ? null
            : Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
    }

    #endregion
}