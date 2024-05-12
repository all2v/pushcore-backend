using System.ComponentModel.DataAnnotations;

namespace Astrum.ITDictionary.Models.Requests;

public class TermRequest
{
    public string Name { get; set; }

    public string Definition { get; set; }

    public Guid CategoryId { get; set; }
}