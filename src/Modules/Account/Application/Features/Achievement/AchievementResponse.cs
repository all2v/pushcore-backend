namespace Astrum.Account.Features.Achievement;

public class AchievementResponse
{
    public Guid Id { get; set; }
    public string? IconUrl { get; set; }
    public Guid? IconId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}