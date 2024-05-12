using Astrum.SharedLib.Persistence.Attributes;

namespace Astrum.SharedLib.Persistence.Utilities;

static internal class AuditUtilities
{
    public static bool IsAuditDisabled(Type type)
    {
        var customAttributes = type.GetCustomAttributes(false);

        return IsAuditDisabled(customAttributes);
    }

    public static bool IsAuditDisabled(Type type, string propertyName)
    {
        if (propertyName == "Discriminator") //set Discriminator shadow property as non auditable
            return false;

        var customAttributes = type.GetProperty(propertyName)?.GetCustomAttributes(false) ?? Array.Empty<object>();

        return IsAuditDisabled(customAttributes);
    }
    private static bool IsAuditDisabled(object[] attributes)
    {
        foreach (var attribute in attributes)
            if (attribute is NotAuditableAttribute notAuditableAttribute)
                return notAuditableAttribute.Enabled;

        return false;
    }
}