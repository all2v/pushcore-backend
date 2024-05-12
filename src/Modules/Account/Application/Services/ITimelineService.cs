using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.Account.Features.Profile.Commands;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services
{
    public interface ITimelineService
    {
        public Task<List<AccessTimeline>> CreateAsync(Guid id);
        public Task<List<AccessTimeline>> UpdateAsync(UserProfile profile, List<EditTimelineCommand> command);
        public Task<Result> DeleteAllAsync();
    }
}
