using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Debts.DomainServices.Repositories;
using Astrum.Debts.Persistence;
using Astrum.Debts.Persistence.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Debts.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomDbContext<DebtsDbContext>(configuration.GetConnectionString("BaseConnection"));
            services.AddScoped<IDebtsRepository, DebtsRepository>();
        }
    }
}
