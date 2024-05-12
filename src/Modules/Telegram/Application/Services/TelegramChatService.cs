using Astrum.Identity.Managers;
using Astrum.News.Repositories;
using Astrum.SharedLib.Common.Results;
using Astrum.Telegram.Application.ViewModels;
using Astrum.Telegram.Domain.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Telegram.Services
{
    public class TelegramChatService : ITelegramChatService
    {
        private readonly IChatRepository _сhatRepository;
        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public TelegramChatService(IChatRepository сhatRepository, ApplicationUserManager userManager, IMapper mapper)
        {
            _сhatRepository = сhatRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<List<ChatForm>>> GetChats(CancellationToken cancellationToken = default)
        {
            var chats = await _сhatRepository.ListAsync(cancellationToken);
            return Result.Success(_mapper.Map<List<ChatForm>>(chats));
        }
        public async Task<Result<ChatForm>> GetChatById(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetTelegramChatById(id);
            var chat = await _сhatRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (chat == null)
            {
                return Result.NotFound("Чат не найден");
            }
            return Result.Success(_mapper.Map<ChatForm>(chat));
        }
        public async Task<Result<ChatForm>> DeleteChat(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetTelegramChatById(id);
            var chat = await _сhatRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (chat == null)
            {
                return Result.NotFound("Ошибка: чат уже удалён!");
            }
            await _сhatRepository.DeleteAsync(chat);
            await _сhatRepository.UnitOfWork.SaveChangesAsync();

            return Result.Success(_mapper.Map<ChatForm>(chat));
        }
    }
}
