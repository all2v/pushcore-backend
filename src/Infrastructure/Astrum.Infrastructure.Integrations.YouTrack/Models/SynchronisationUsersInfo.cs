using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models
{
    public class SynchronisationUsersInfo
    {
        public SynchronisationUsersInfo(List<SynchronisationUser> users, List<TrackerUserGroup> userGroups)
        {
            Users = users;
            UserGroups = userGroups;
        }
        
        public List<SynchronisationUser> Users { get; set; }
        public List<TrackerUserGroup> UserGroups { get; set; }
    }

    public class SynchronisationUser
    {
        public SynchronisationUser(string ringId, string email)
        {
            RingId = ringId;
            Email = email;
        }

        public string RingId { get; set; }
        public string Email { get; set; }
    }

    public class TrackerUserGroup
    {
        public TrackerUserGroup(){}
        public TrackerUserGroup(string id, List<string> userIds, string name)
        {
            Id = id;
            Users = userIds;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<string?> Users { get; set; }
    }
}
