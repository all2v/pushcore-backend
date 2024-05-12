using Astrum.CodeRev.Application.UserService.ViewModel.DTO.SyncInterviews;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public interface IMeetsService
{
    public Task<Result<List<MeetInfoDto>>> GetMeets(Guid requestingUserId, int offset, int limit);
}