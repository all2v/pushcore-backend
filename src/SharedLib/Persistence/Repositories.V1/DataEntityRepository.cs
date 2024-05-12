using Astrum.Core.Application.Repositories;
using Astrum.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using IUnitOfWork = Astrum.Core.Application.Contracts.Persistence.Repositories.IUnitOfWork;

namespace Astrum.Core.Persistence.Repositories.V1;

/// <inheritdoc cref="IDataEntityRepository{T,TId}" />
public class DataEntityRepository<TEntity, TId, TDbContext>
    : EFRepository<TEntity, TId, TDbContext>, IDataEntityRepository<TEntity, TId>
    where TEntity : class, IDataEntity<TId>
    where TDbContext : DbContext, IUnitOfWork
{
    public DataEntityRepository(TDbContext context) : base(context)
    {
    }
}