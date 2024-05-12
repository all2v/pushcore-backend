namespace Astrum.SharedLib.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Line1;
        yield return Line2;
        yield return City;
        yield return Postcode;
        yield return Country;
    }
}