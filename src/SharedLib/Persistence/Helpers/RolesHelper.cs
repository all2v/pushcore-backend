using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Resources;
using Astrum.SharedLib.Domain.Enums;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Astrum.SharedLib.Persistence.Helpers
{
    public static class RolesHelper
    {

        //TODO: move to automapper profiles
        public static IEnumerable<RolesEnum> MapToEnumRoles(IEnumerable<string> roles) 
        {
            var result = new List<RolesEnum>();
            foreach (var role in roles)
            {
                var strRole = role switch
                {
                    "Trainee" => RolesEnum.Trainee,
                    "Employee" => RolesEnum.Employee,
                    "Manager" => RolesEnum.Manager,
                    "Admin" => RolesEnum.Admin,
                    "SuperAdmin" => RolesEnum.SuperAdmin,
                };
                result.Add(strRole);
            }
            return result;
        }

        public static IEnumerable<string> MapToStringRoles(IEnumerable<RolesEnum> roles)
        {
            var result = new List<string>();
            foreach (var role in roles)
            {
                var strRole = role switch
                {
                    RolesEnum.Trainee => "Trainee",
                    RolesEnum.Employee => "Employee",
                    RolesEnum.Manager => "Manager",
                    RolesEnum.Admin => "Admin",
                    RolesEnum.SuperAdmin => "SuperAdmin",
                };
                result.Add(strRole);
            }
            return result;
        }
    }
}
