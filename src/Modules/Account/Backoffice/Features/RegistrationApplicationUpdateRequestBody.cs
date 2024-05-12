namespace Astrum.Account.Features;

public record RegistrationApplicationUpdateRequestBody
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}