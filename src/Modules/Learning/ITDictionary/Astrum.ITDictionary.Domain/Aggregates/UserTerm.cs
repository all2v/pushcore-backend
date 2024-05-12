using Astrum.SharedLib.Domain.Entities;

namespace Astrum.ITDictionary.Aggregates;

public class UserTerm
{
    public Guid UserId { get; set; }

    public Guid TermId { get; set; }

    public Term Term { get; set; }

    public bool IsSelected { get; set; }
}