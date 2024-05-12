using Astrum.Account.Domain.Aggregates;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates;

public class UserProfile : AggregateRootBase<Guid>
{
    public Guid UserId { get; set; }
    public Guid BasketId { get; set; }
    public SocialNetworks SocialNetworks { get; set; }
    public Guid SocialNetworksId { get; set; }
    public Guid ActiveTimeline { get; set; }
    public List<AccessTimeline> Timelines { get; set; }
    public List<CustomField>? CustomFields { get; set; }
    public virtual ICollection<Achievement> Achievements { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    /// <summary>
    ///     Отчество
    /// </summary>
    public string Patronymic { get; set; }
    public string? Address { get; set; }
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    ///     Должность
    /// </summary>
    public Guid? PositionId { get; set; }
    public Position? Position { get; set; }
    public List<string>? Competencies { get; set; }
    public Guid? AvatarImageId { get; set; }
    public Guid? CoverImageId { get; set; }

    public string? RequisiteBank { get; set; }
    public string? RequisiteNumberPhone { get; set; }

}