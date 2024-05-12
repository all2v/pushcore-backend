using Microsoft.EntityFrameworkCore;

namespace Astrum.SharedLib.Persistence.ModelBuilders;

/// <summary>
/// </summary>
public interface ICustomModelBuilder
{
    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    void Build(ModelBuilder builder);
}