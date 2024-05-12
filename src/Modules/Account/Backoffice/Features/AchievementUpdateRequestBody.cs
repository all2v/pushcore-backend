namespace Astrum.Account.Features;

public record AchievementUpdateRequestBody
{
    /// <summary>
    ///     Url to icon image
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    ///     Achievement name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Description of an achievement
    /// </summary>
    public string Description { get; set; }
}