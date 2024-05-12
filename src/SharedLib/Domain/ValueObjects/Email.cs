
namespace Astrum.SharedLib.Domain.ValueObjects;

public class Email : ValueObject
{
    private long Id { get; set; }
    private string Content { get; set; }
    private bool IsDefault { get; set; }

    //TemplateType? TemplateType { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Content;
        yield return IsDefault;
    }
}