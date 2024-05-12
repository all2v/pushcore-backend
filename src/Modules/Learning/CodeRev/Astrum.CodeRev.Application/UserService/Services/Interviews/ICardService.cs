using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public interface ICardService
{
    Task<Result<List<CardInfo>>> GetCards(int offset, int limit);
}