using Ardalis.GuardClauses;

namespace Astrum.SharedLib.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    #region Fields

    public string Value { get; }

    #endregion

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    #region Constructor

    private PhoneNumber() { }

    public PhoneNumber(string number)
    {
        Guard.Against.NullOrWhiteSpace(number, nameof(number));

        Value = number;
    }

    #endregion
}