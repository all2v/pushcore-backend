namespace Astrum.ITDictionary.Models.Requests;

public class UserTermsRequest
{
    public Guid UserId { get; set; }

    public List<Guid> TermIds { get; set; }
}