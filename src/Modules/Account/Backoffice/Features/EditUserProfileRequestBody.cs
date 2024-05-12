namespace Astrum.Account.Features;

public class EditUserProfileRequestBody
{
    public EditContactsRequestBody? Contacts { get; set; }
    public EditSocialNetworksRequestBody? SocialNetworks { get; set; }
    public EditTimelineRequestBody? ActiveTimeline { get; set; }
    public List<EditTimelineRequestBody>? Timelines { get; set; }
    public string? Username { get; set; }
    public string? Address { get; set; }
    public List<string>? Competencies { get; set; }
}