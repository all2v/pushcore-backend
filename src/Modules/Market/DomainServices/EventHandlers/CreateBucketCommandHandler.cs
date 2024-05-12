using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.DomainServices.Features.Commands;
using Astrum.IdentityServer.Domain.Events;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.Market.Aggregates;
using Astrum.Market.Repositories;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Domain.EventHandlers;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Astrum.Market.DomainServices.EventHandlers
{
    public class CreateBucketCommandHandler : CommandHandler<CreateBucketCommand, Result<Guid>>
    {
        private readonly IMarketBasketRepository _marketBasketRepository;

        public CreateBucketCommandHandler(IMarketBasketRepository marketBasketRepository)
        {
            _marketBasketRepository = marketBasketRepository;
        }

        public override async Task<Result<Guid>> Handle(CreateBucketCommand @event, CancellationToken cancellationToken)
        {
            var aggregate = new MarketBasket(@event.UserId);
            
            await _marketBasketRepository.AddAsync(aggregate);
            await _marketBasketRepository.UnitOfWork.SaveChangesAsync();

            return aggregate.Id;
        }
    }
}
