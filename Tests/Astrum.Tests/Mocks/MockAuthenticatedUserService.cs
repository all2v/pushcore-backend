using Astrum.Identity.Contracts;
using Astrum.SharedLib.Domain.Enums;

namespace Astrum.Tests.Mocks;

internal class MockAuthenticatedUserService : IAuthenticatedUserService
{
    #region IAuthenticatedUserService Members

    public Guid? UserId => Guid.Empty;

    public string Username => "Mr.Tester";

    public string Name => "Tester Tester";

    public IEnumerable<RolesEnum> Roles { get; set; } = new List<RolesEnum>();

    #endregion
}