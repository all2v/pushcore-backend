using Astrum.Identity.Contracts;
using Astrum.SharedLib.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace HRM.UnitTests
{
    internal class ContextGetter
    {
        public T GetContext<T>() where T : DbContext
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IAuthenticatedUserService, MockAuthenticatedUserService>();
            var currentAssembly = typeof(ContextGetter).Assembly;
            services.AddMediatR(currentAssembly);
            services.AddDbContext<T>(options => options.UseInMemoryDatabase("RecrutingDb"));

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<T>()!;
        }

        public class MockAuthenticatedUserService : IAuthenticatedUserService
        {

            public Guid? UserId => Guid.Empty;

            public string Username => "shiryaeva";

            public string Name => "shiryaeva";

            public IEnumerable<RolesEnum> Roles { get; set; } = new List<RolesEnum>();

        }
    }
}
