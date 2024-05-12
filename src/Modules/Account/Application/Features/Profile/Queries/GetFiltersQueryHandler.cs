using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Application.Repositories;
using Astrum.Account.Application.ViewModels;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetFiltersQueryHandler : QueryResultHandler<GetFiltersQuery, FilterResponse>
    {
        private readonly IPositionRepository _positionRepository;

        public GetFiltersQueryHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public override async Task<Result<FilterResponse>> Handle(GetFiltersQuery query, CancellationToken cancellationToken = default)
        {
            //TODO: move expression to spec
            var positions = await _positionRepository.Items.Include(p => p.UserProfiles).ToListAsync(cancellationToken);

            var block = new FilterBlock { Name = "positionIds", Title = "Фильтры" };
            var resultBlocks = new List<FilterBlock> { block };
            block.FilterItems = positions
                .Where(p => p.UserProfiles.Any())
                .Select(position => new FilterItem()
                {
                    Value = position.Id.ToString(),
                    Title = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(position.Name),
                    Count = position.UserProfiles.Count()
                })
                .OrderByDescending(info => info.Count)
                .ToList();

            return Result.Success(new FilterResponse { Blocks = resultBlocks });
        }
    }
}
