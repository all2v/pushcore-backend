// using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
// 
// using Astrum.SharedLib.Application.Repositories;
// using Astrum.SharedLib.Domain.Interfaces;
// using Microsoft.EntityFrameworkCore;
//
// namespace Astrum.SharedLib.Persistence.Repositories;
//
// /// <inheritdoc cref="IDataEntityRepository{TEntity,TId}" />
// public class DataEntityRepository<TEntity, TId, TDbContext>
//     : EFRepository<TEntity, TId, TDbContext>, IDataEntityRepository<TEntity, TId>
//     where TEntity : class, IDataEntity<TId>
//     where TDbContext : DbContext, IUnitOfWork
// {
//     public DataEntityRepository(TDbContext context) : base(context)
//     {
//     }
// }