using Astrum.Market.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Market.Repositories;

public interface IMarketBasketRepository : IEntityRepository<MarketBasket, Guid>
{
}