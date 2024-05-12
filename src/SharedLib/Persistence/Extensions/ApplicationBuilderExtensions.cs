using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.SharedLib.Persistence.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UpdateDatabase<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<TDbContext>();
        context?.Database.Migrate();
    }
}