using Astrum.Account.Aggregates;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.CustomField;
using Astrum.Identity.Models;
using Astrum.News.ViewModels;
using Astrum.Projects.ViewModels.Views;

namespace Astrum.Account.Features.Profile;

public class UserProfileResponse
{
    public List<AchievementResponse> Achievements { get; set; }
    public List<string> Competencies { get; set; }
    public ContactsResponse Contacts { get; set; }
    public SocialNetworksResponse SocialNetworks { get; set; }
    public List<MemberShortView> Projects { get; set; }
    public List<CustomFieldResponse> CustomFields { get; set; }
    public Guid UserId { get; set; }
    public Guid BasketId { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    /// <summary>
    ///     Отчество
    /// </summary>
    public string Patronymic { get; set; }

    public string Address { get; set; }
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    ///     Должность
    /// </summary>
    public string Position { get; set; }

    public int Money { get; set; }

    public AccessTimelineResponse ActiveTimeline { get; set; }

    public List<AccessTimelineResponse> Timelines { get; set; }
}