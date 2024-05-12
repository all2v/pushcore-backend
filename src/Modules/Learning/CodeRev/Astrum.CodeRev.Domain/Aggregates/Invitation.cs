using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

    public class Invitation : AggregateRootBase<Guid>
    {
        public Role Role { get; set; }
        public Guid InterviewId { get; set; }
        public Interview Interview { get; set; }
        public long ExpiredAt { get; set; }
        //public Guid CreatedBy { get; set; }
        public bool IsSynchronous { get; set; }
    }
