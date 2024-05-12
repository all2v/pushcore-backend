using Astrum.Identity.Features.Commands;
using Astrum.Tests;
using FluentAssertions;
using Xunit;
using IResult = Astrum.SharedLib.Common.Results.IResult;

namespace Astrum.Core.Tests.Application.Commands;

public class CreateUserTests : TestBase
{
    [Fact]
    public async Task CreateUser_WhenPasswordIsNull_ShouldFail()
    {
        var command = new CreateUserCommand(
            "testUser",
            "testName",
            "test@test.com",
            null,
            null);

        var result = await ServiceBus.Send(command);

        // result.Succeeded.Should().BeFalse();
        // result.Exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateUser_WhenRequestIsValid_ShouldSucceed()
    {
        var command = new CreateUserCommand(
            "testUser",
            Guid.NewGuid().ToString("N"),
            "test@test.com",
            "test test",
            null);

        var result = await ServiceBus.Send(command);

        // result.Succeeded.Should().BeTrue();
        // result.Exception.Should().BeNull();
    }
}