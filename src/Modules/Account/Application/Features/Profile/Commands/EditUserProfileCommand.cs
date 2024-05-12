using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.Account.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Profile.Commands;

public class EditUserProfileCommand : CommandResult<EditUserProfileResponse>
{
    [FromQuery]
    public string Username { get; set; }

    public string? NewUsername { get; set; }
    public EditContactsCommand? Contacts { get; set; }
    public EditSocialNetworksCommand? SocialNetworks { get; set; }
    public EditTimelineCommand? ActiveTimeline { get; set; }
    public List<EditTimelineCommand?> Timelines { get; set; }
    public string? Address { get; set; }
    public List<string>? Competencies { get; set; }
    public string? RequisiteBank { get; set; }
    public string? RequisiteNumberPhone { get; set; }
}

public class EditContactsCommand : CommandResult
{
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}

public class EditSocialNetworksCommand : CommandResult
{
    public string? Behance { get; set; }
    public string? Figma { get; set; }
    public string? GitHub { get; set; }
    public string? GitLab { get; set; }
    public string? Instagram { get; set; }
    public string? Telegram { get; set; }
    public string? VK { get; set; }
}

public class EditTimelineCommand : CommandResult
{
    public TimelineType? TimelineType { get; set; }
    public List<EditTimelineIntervalCommand>? Intervals { get; set; }
}

public class EditTimelineIntervalCommand : CommandResult
{
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public TimelineIntervalType? IntervalType { get; set; }
}