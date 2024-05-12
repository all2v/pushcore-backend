namespace Astrum.Account.Features.Profile;

public class EditUserProfileResponse
{
    public ContactsResponse? Contacts { get; set; }
    public SocialNetworksResponse? SocialNetworks { get; set; }
    public AccessTimelineResponse ActiveTimeline { get; set; }
    public List<AccessTimelineResponse> Timelines { get; set; }
    public string? Username { get; set; }
    public string? Address { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Position { get; set; }
    public string? AvatarUrl { get; set; }
    public List<string>? Competencies { get; set; }
    public string? RequisiteBank { get; set; }
    public string? RequisiteNumberPhone { get; set; }
}