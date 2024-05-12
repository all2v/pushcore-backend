using System.Security.Cryptography;
using Ardalis.Specification;
using Astrum.Logging.Entities;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Logging.Repositories
{
    public interface ILogHttpRepository : IEntityRepository<LogHttp, Guid> 
    {
    }
}
