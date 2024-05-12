using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.Account.Enums;
using Astrum.Account.Features.Profile.Commands;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.UserProfile;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services
{
    public class TimelineService : ITimelineService
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IUserProfileRepository _profileRepository;

        public TimelineService(ITimelineRepository timelineRepository, IUserProfileRepository profileRepository)
        {
            _timelineRepository = timelineRepository;
            _profileRepository = profileRepository;
        }

        public async Task<List<AccessTimeline>> CreateAsync(Guid id)
        {
            var timelines = new List<AccessTimeline>();
            foreach (TimelineType type in Enum.GetValues(typeof(TimelineType)))
            {
                var timeline = new AccessTimeline
                {
                    UserId = id,
                    TimelineType = type,
                    Intervals = new List<AccessTimelineInterval>()
                };
                var intervalType = type == TimelineType.Available ? TimelineIntervalType.Available :
                    type == TimelineType.BusinessTrip ? TimelineIntervalType.MessageOnly : TimelineIntervalType.Unavailable;
                var interval = new AccessTimelineInterval
                {
                    StartTime = new TimeSpan(0, 0, 0),
                    EndTime = new TimeSpan(0, 0, 0),
                    TimelineId = timeline.Id,
                    IntervalType = intervalType
                };
                timeline.Intervals.Add(interval);
                timeline = await _timelineRepository.AddAsync(timeline);
                timelines.Add(timeline);
            }

            await _timelineRepository.UnitOfWork.SaveChangesAsync();
            return timelines;
        }

        public async Task<List<AccessTimeline>> UpdateAsync(UserProfile profile, List<EditTimelineCommand> command)
        {
            var spec = new GetTimelineByUserIdSpecification(profile.UserId);
            var timelines = await _timelineRepository.ListAsync(spec);
            if (timelines == null)
                throw new Exception();
            foreach (var timeline in timelines)
            {
                var edited = command.FirstOrDefault(x => x.TimelineType == timeline.TimelineType);
                timeline.TimelineType = edited?.TimelineType ?? timeline.TimelineType;
                if (edited?.Intervals != null)
                    timeline.Intervals = edited.Intervals.Select(i => new AccessTimelineInterval
                    {
                        StartTime = (TimeSpan)i.StartTime,
                        EndTime = (TimeSpan)i.EndTime,
                        IntervalType = (TimelineIntervalType)i.IntervalType,
                    }).ToList();
            }
            

            return timelines;
        }

        public async Task<Result> DeleteAllAsync()
        {
            var timelines = await _timelineRepository.ListAsync();
            await _timelineRepository.DeleteRangeAsync(timelines);
            await _timelineRepository.UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
