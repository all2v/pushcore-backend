using Astrum.Identity.Models;
using AutoMapper;

namespace Astrum.Identity;

public class UserContextAccessor : IUserContextAccessor
{
    private readonly IApplicationUserAccessor _applicationUserAccessor;

    private readonly IMapper _mapper;
    private ApplicationUser? _currentUser;

    public UserContextAccessor(IMapper mapper, IApplicationUserAccessor applicationUserAccessor)
    {
        _applicationUserAccessor = applicationUserAccessor;
        _mapper = mapper;
    }

    #region IUserContextAccessor Members

    public async Task<ApplicationUser?> GetUser()
    {
        if (_currentUser != null) return _currentUser;
        _currentUser = await _applicationUserAccessor.GetUser();
        ;

        return _currentUser;
    }

    #endregion
}