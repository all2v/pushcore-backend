namespace Astrum.Application.Models;

public record ApplicationConfigurationDto(string Id, string Value, string Description, bool IsEncrypted)
{
    public string Id { get; set; } = Id;
    public string Value { get; set; } = Value;
    public string Description { get; set; } = Description;
    public bool IsEncrypted { get; set; } = IsEncrypted;
}