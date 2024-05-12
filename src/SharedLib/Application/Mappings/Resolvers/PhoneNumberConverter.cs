using Astrum.SharedLib.Domain.ValueObjects;
using AutoMapper;

namespace Astrum.SharedLib.Application.Mappings.Resolvers;

public class StringToPhoneNumberConverter : ITypeConverter<string, PhoneNumber>
{
    #region ITypeConverter<string,PhoneNumber> Members

    public PhoneNumber Convert(string source, PhoneNumber destination, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source)) 
            throw new Exception($"Cannot convert {source} to PhoneNumber");

        return new PhoneNumber(source);
    }

    #endregion
}

public class PhoneNumberToStringConverter : ITypeConverter<PhoneNumber, string>
{
    #region ITypeConverter<PhoneNumber,string> Members

    public string Convert(PhoneNumber sourceMember, string destination, ResolutionContext context)
    {
        return sourceMember.Value;
    }

    #endregion
}