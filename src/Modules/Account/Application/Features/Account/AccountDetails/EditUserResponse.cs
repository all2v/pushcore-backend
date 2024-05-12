using Astrum.SharedLib.Domain.Enums;

namespace Astrum.Account.Features.Account.AccountDetails;

public class EditUserResponse
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public IEnumerable<RolesEnum> Roles { get; set; }

    public bool IsActive { get; set; }
}
// public class EditUserResponse
// {
//     public Guid Id { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Username)]
//     public string Username { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Password)]
//     [MinLength(10, ErrorMessage = ResourceKeys.Validations_FieldLength)]
//     [DataType(DataType.Password)]
//     public string Password { get; set; }
//
//     [Required(ErrorMessage = ResourceKeys.Validations_Required)]
//     [Display(Name = ResourceKeys.Labels_Name)]
//     public string Name { get; set; }
//
//     [Required(ErrorMessage = ResourceKeys.Validations_Required)]
//     [Display(Name = ResourceKeys.Labels_Email)]
//     [EmailAddress(ErrorMessage = ResourceKeys.Validations_EmailFormat)]
//     public string Email { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Phone)]
//     public string PhoneNumber { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Roles)]
//     [Required(ErrorMessage = ResourceKeys.Validations_Required)]
//     public IEnumerable<RolesEnum> Roles { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Active)]
//     [Required(ErrorMessage = ResourceKeys.Validations_Required)]
//     public bool IsActive { get; set; }
// }