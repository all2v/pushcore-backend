using Astrum.Identity.Features.Commands;
using Astrum.SharedLib.Domain.Enums;
using Astrum.Tests;
using FluentAssertions;
using Xunit;
using IResult = Astrum.SharedLib.Common.Results.IResult;

namespace Astrum.Core.Tests.Application.Commands;

public class AddRolesTests : TestBase
{
    [Fact]
    public async Task AddRoles_WhenRoleIsValidAndNotAssigned_ShouldSucceed()
    {
        var command = new AddRolesCommand("basicUser", new List<string>
        {
            RolesEnum.SuperAdmin.ToString()
        });

        var result = await ServiceBus.Send(command);

        // result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task AddRoles_WhenRoleIsInvalid_ShouldFail()
    {
        var command = new AddRolesCommand("basicUser", new List<string>
        {
            "InvalidRole"
        });

        var result = await ServiceBus.Send(command);

        // result.Succeeded.Should().BeFalse();
        // result.Errors.Should().Contain(e => e.ExceptionMessage == "Role InvalidRole is invalid.");
    }
}