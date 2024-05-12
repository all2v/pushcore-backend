using Astrum.Account.Aggregates;
using Astrum.Account.Features.Profile.Commands;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.SocialNetworks;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public class SocialNetworksService : ISocialNetworksService
{
    private readonly ISocialNetworksRepository _socialNetworksRepository;

    public SocialNetworksService(ISocialNetworksRepository socialNetworksRepository)
    {
        _socialNetworksRepository = socialNetworksRepository;
    }

    #region ISocialNetworksService Members

    public async Task<Result<SocialNetworks>> UpdateAsync(UserProfile profile, EditSocialNetworksCommand command)
    {
        var getSocialNetworksByIdSpec = new GetSocialNetworksByIdSpec(profile.SocialNetworksId);
        var socialNetworks = await _socialNetworksRepository.FirstOrDefaultAsync(getSocialNetworksByIdSpec);
        if (socialNetworks == null)
            throw new Exception();

        socialNetworks.Behance = command?.Behance ?? socialNetworks.Behance;
        socialNetworks.Figma = command?.Figma ?? socialNetworks.Figma;
        socialNetworks.GitHub = command?.GitHub ?? socialNetworks.GitHub;
        socialNetworks.GitLab = command?.GitLab ?? socialNetworks.GitLab;
        socialNetworks.Instagram = command?.Instagram ?? socialNetworks.Instagram;
        socialNetworks.Telegram = command?.Telegram ?? socialNetworks.Telegram;
        socialNetworks.VK = command?.VK ?? socialNetworks.VK;
        try
        {
            await _socialNetworksRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении социальных сетей");
        }

        return Result.Success(socialNetworks);
    }

    #endregion
}