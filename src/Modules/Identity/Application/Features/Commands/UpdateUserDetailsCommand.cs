using Astrum.Identity.Contracts;
using Astrum.Identity.DTOs;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Identity.Features.Commands;

public class UpdateUserDetailsCommand : CommandResult 
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string PrimaryPhone { get; init; }
    public string SecondaryPhone { get; init; }
}

/// <summary>
///     Update User Details Command Handler
/// </summary>
public class UpdateUserDetailsCommandHandler : CommandResultHandler<UpdateUserDetailsCommand>
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMapper _mapper;

    public UpdateUserDetailsCommandHandler(IApplicationUserService applicationUserService, IMapper mapper)
    {
        _applicationUserService = applicationUserService;
        _mapper = mapper;
    }

    public override async Task<Result> Handle(UpdateUserDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        return await _applicationUserService.UpdateUserDetails(_mapper.Map<UpdateUserDetailsDto>(command));
    }
}