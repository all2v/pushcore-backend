using System.ComponentModel.DataAnnotations;
using Astrum.Account.Services;
using Astrum.Recruting.Application.Shared;
using Astrum.Recruting.Domain.Aggregates.Board;
using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.Recruting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public interface IKanbanService
{
    //Создает дефолтные 3 стадии по умолчанию
    Task<BoardView> CreateBoard(Guid vacancyId, List<StageCreationModel>? stages = null);
    public Task<KanbanBoard> CreateDefaultBoardForVacancy(Guid vacancyId);

    Task<BoardView?> GetBoard(Guid vacancyId);

    //можно редактиовать стадию или создать
    Task<BoardView> CreateOrUpdateStage(StageCreationModel stageCreationModel);

    //если на стадии есть карточки то удалить нельзя
    Task<BoardView> DeleteStage(Guid stageId);
    Task<BoardView> SetCandidateToStage(StageCardModel stageCreationModel);
    Task<List<CardEventModel>> CreateOrUpdateCardEvent(CardEventModel cardEventModel);
    Task<List<StageCardModel>> MoveCardToStage(Guid stageCardId, Guid stageId);
    Task<List<AccountableModel>> GetAccountables();
    Task CreateComment(CommentModel commentModel);
}

public class KanbanService : IKanbanService
{
    private readonly RecrutingDbContext _context;
    private readonly IUserProfileService _userProfileService;

    public KanbanService(RecrutingDbContext context, IUserProfileService userProfileService)
    {
        _context = context;
        _userProfileService = userProfileService;
    }


    public async Task<BoardView> CreateBoard(Guid vacancyId, List<StageCreationModel>? stages = null)
    {
        if (await _context.KanbanBoards.AnyAsync(x => x.VacancyId == vacancyId) ||
            !await _context.Vacancies.AnyAsync(x => x.Id == vacancyId))
            throw new ArgumentException();
        var board = new KanbanBoard
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.Date,
            VacancyId = vacancyId,
            Stages = stages.Select(x => new Stage
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTimeOffset.UtcNow.Date,
                    Position = x.Position,
                    Title = x.Name,
                    StageCards = x.CardModels
                        .Select(x => new StageCard
                        {
                            HrmUserId = x.HrmUserId,
                            Id = Guid.NewGuid()
                        })
                        .ToList()
                })
                .ToList()
        };
        await _context.AddAsync(board);
        await _context.SaveChangesAsync();
        return new BoardView
        {
            BoardId = board.Id,
            Stages = board.Stages.Select(x => new StageCreationModel
            {
                Position = x.Position,
                Name = x.Title,
                BoardId = x.BoardId,
                StageId = x.Id,
                CardModels = x.StageCards
                    .Select(c => new StageCardModel
                    {
                        StageId = c.StageId,
                        HrmUserId = c.HrmUserId,
                        StageCardId = c.Id
                    })
                    .ToList()
            }).ToList()
        };
    }

    public async Task<BoardView?> GetBoard(Guid vacancyId)
    {
        var stageIds = await _context.KanbanBoards
            .Where(x => x.VacancyId == vacancyId)
            .SelectMany(x => x.Stages.Select(x => x.Id))
            .ToListAsync();
        var accountableIds = await _context.StageCards
            .AsNoTracking()
            .Include(x => x.CardEvents)
            .Where(x => stageIds.Contains(x.StageId))
            .ToListAsync();

        var a = (await _userProfileService.GetUsersProfilesSummariesAsync(
            accountableIds.SelectMany(x => x.CardEvents.Select(x => x.AccountableId)))).Data;

        return await _context.KanbanBoards
            .Where(x => x.VacancyId == vacancyId)
            .Select(board => new BoardView
            {
                BoardId = board.Id,
                Stages = board.Stages.Select(x => new StageCreationModel
                {
                    Position = x.Position,
                    Name = x.Title,
                    BoardId = x.BoardId,
                    StageId = x.Id,
                    CardModels = x.StageCards
                        .Select(c => new StageCardModel
                        {
                            StageId = c.StageId,
                            HrmUserId = c.HrmUserId,
                            StageCardId = c.Id,
                            Events = c.CardEvents.Select(x => new CandidateEventModel
                            {
                                Date = x.EventDateTime,
                                Title = x.Name,
                                EventId = x.Id,
                            }).ToList()
                        })
                        .ToList()
                }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<BoardView> CreateOrUpdateStage(StageCreationModel stageCreationModel)
    {
        var stage = await _context.Stages.FirstOrDefaultAsync(x => stageCreationModel.StageId == x.Id);
        if (stage == null)
        {
            stage = new Stage
            {
                Id = Guid.NewGuid(),
                Position = stageCreationModel.Position,
                Title = stageCreationModel.Name,
                CreatedAt = DateTimeOffset.UtcNow,
                BoardId = stageCreationModel.BoardId
            };
            await _context.AddAsync(stage);
        }
        else
        {
            stage.Position = stageCreationModel.Position;
            stage.Title = stageCreationModel.Name;
        }

        await _context.SaveChangesAsync();

        return await GetBoard(stageCreationModel.BoardId);
    }

    public async Task<BoardView> DeleteStage(Guid stageId)
    {
        var stage = await _context.Stages.Include(x => x.StageCards).FirstOrDefaultAsync(x => stageId == x.Id);
        stage.AssertFound();
        if (stage.StageCards.Any())
            throw new ArgumentException();
        var boardId = stage.BoardId;
        _context.Stages.Remove(stage);
        await _context.SaveChangesAsync();
        return await GetBoard(boardId);
    }

    public async Task<BoardView> SetCandidateToStage(StageCardModel stageCreationModel)
    {
        var stage = await _context.Stages
            .FirstOrDefaultAsync(x => x.Id == stageCreationModel.StageId);
        stage.AssertFound();
        var oldPosition = await _context.KanbanBoards
            .Where(x => x.Id == stage.BoardId)
            .SelectMany(x => x.Stages.SelectMany(x => x.StageCards))
            .Where(x => x.HrmUserId == stageCreationModel.HrmUserId)
            .FirstOrDefaultAsync();
        oldPosition.StageId = stage.Id;
        await _context.SaveChangesAsync();
        return null;
    }

    public async Task<KanbanBoard> CreateDefaultBoardForVacancy(Guid vacancyId)
    {
        var tmp = await _context.PracticeVacancies
            .Where(x => x.Id == vacancyId)
            .SelectMany(x => x.PracticeRequests.Select(x => x.HrmUserId))
            .Distinct()
            .ToListAsync();
        if (tmp == null)
        {
            var resumeIds = await _context.VacancyCandidates
                .Where(x => x.VacancyId == vacancyId)
                .Select(x => x.CandidateResumeId)
                .ToListAsync();

            tmp = await _context.CandidateResumes
                .Where(x => resumeIds.Contains(x.Id))
                .Select(x => x.HrmUserId)
                .Distinct()
                .ToListAsync();
        }

        tmp.AssertFound();

        var board = new KanbanBoard
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.Date,
            VacancyId = vacancyId,
        };
        await _context.AddAsync(board);

        var stages = new List<Stage>
        {
            new Stage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.Date,
                Position = 0,
                BoardId = board.Id,
                Title = "Новые",
                KanbanBoard = board,
                StageCards = tmp
                    .Select(x => new StageCard
                    {
                        HrmUserId = x,
                        Id = Guid.NewGuid(),
                    })
                    .ToList()
            },
            new Stage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.Date,
                Position = 1,
                BoardId = board.Id,
                Title = "В работе",
                KanbanBoard = board
            },
            new Stage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.Date,
                Position = 2,
                BoardId = board.Id,
                Title = "Отклоненно",
                KanbanBoard = board
            },
        };
        await _context.AddRangeAsync(stages);
        board.Stages = stages;
        await _context.SaveChangesAsync();
        return board;
    }

    public async Task<List<CardEventModel>> CreateOrUpdateCardEvent(CardEventModel cardEventModel)
    {
        var stageCard = await _context.StageCards
            .Include(x => x.CardEvents)
            .FirstOrDefaultAsync(x => x.Id == cardEventModel.StageCardId);

        stageCard.AssertFound();

        StageEvent cardEvent;
        if (cardEventModel.Id == null)
        {
            cardEvent = new StageEvent
            {
                Name = cardEventModel.Name,
                AccountableId = cardEventModel.AccountableId,
                StageCardId = cardEventModel.StageCardId,
                EventDateTime = cardEventModel.EventDateTime,
                StageId = cardEventModel.StageId,
                StageCard = stageCard
            };
            await _context.AddAsync(cardEvent);
        }
        else
        {
            cardEvent = stageCard.CardEvents.First(x => x.Id == cardEventModel.Id);
            _context.Attach(cardEvent);

            cardEvent.Name = cardEventModel.Name;
            cardEvent.AccountableId = cardEventModel.AccountableId;
            cardEvent.StageCardId = cardEventModel.StageCardId;
            cardEvent.EventDateTime = cardEventModel.EventDateTime;
            cardEvent.StageId = cardEventModel.StageId;
            cardEvent.StageCard = stageCard;
        }

        await _context.SaveChangesAsync();
        return await _context.StageEvents
            .Where(x => x.StageId == cardEventModel.StageCardId)
            .Select(x => new CardEventModel
            {
                Id = x.Id,
                Name = x.Name,
                AccountableId = x.AccountableId,
                StageCardId = x.StageCardId,
                EventDateTime = x.EventDateTime,
                StageId = x.StageId
            })
            .ToListAsync();
    }

    public async Task<List<StageCardModel>> MoveCardToStage(Guid stageCardId, Guid stageId)
    {
        var stageCard = await _context.StageCards.FirstOrDefaultAsync(x => x.Id == stageCardId);
        stageCard.AssertFound();
        var stage = await _context.Stages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == stageId);
        stage.AssertFound();
        stageCard.StageId = stage.Id;
        stageCard.Stage = stage;
        await _context.SaveChangesAsync();

        var accountableIds = await _context.StageCards
            .AsNoTracking()
            .Include(x => x.CardEvents)
            .Where(x => x.StageId == stageId)
            .ToListAsync();
        var a = (await _userProfileService.GetUsersProfilesSummariesAsync(
            accountableIds.SelectMany(x => x.CardEvents.Select(x => x.AccountableId)))).Data;

        return accountableIds
            .Select(x =>
            {
                return new StageCardModel
                {
                    StageId = x.StageId,
                    HrmUserId = x.HrmUserId,
                    StageCardId = x.Id,
                    Events = x.CardEvents.Select(sEvent => new CandidateEventModel
                        {
                            Date = sEvent.EventDateTime,
                            Title = sEvent.Name,
                            EventId = sEvent.Id,
                            AccountableFio = a.FirstOrDefault(a => a.UserId == sEvent.AccountableId)?.NameWithSurname
                        })
                        .ToList()
                };
            })
            .ToList();
    }

    public async Task<List<AccountableModel>> GetAccountables()
    {
        return (await _userProfileService.GetAllUsersProfilesSummariesAsync())
            .Data
            .Select(x => new AccountableModel
            {
                FirstName = x.Name,
                Surname = x.Surname,
                LastName = null,
                AccountableId = x.UserId
            })
            .ToList();
    }

    public async Task CreateComment(CommentModel commentModel)
    {
        var card = await _context.StageCards.FirstOrDefaultAsync(x => x.Id == commentModel.StageCardId);
        var vacancyId = await _context.Stages
            .Where(x => x.Id == card.StageId)
            .Select(x => x.KanbanBoard.VacancyId)
            .FirstOrDefaultAsync();
        card.AssertFound();
        var comment = new UserComment
        {
            Id = Guid.NewGuid(),
            HrmUserId = card.HrmUserId,
            Comment = commentModel.Comment,
            StageId = card.StageId,
            VacancyId = vacancyId
        };
        await _context.AddAsync(comment);
    }
}

public class UserCommentModel
{
    public Guid Id { get; set; }
    public Guid HrmUserId { get; set; }
    public string Comment { get; set; }
    public string Stage { get; set; }
    public string Vacancy { get; set; }
}

public class CardEventModel
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset EventDateTime { get; set; }
    public Guid AccountableId { get; set; }
    public Guid StageCardId { get; set; }
    public Guid StageId { get; set; }
}

public class StageCreationModel
{
    public Guid StageId { get; set; }
    public string Name { get; set; }
    public Guid BoardId { get; set; }
    public int Position { get; set; }
    public IEnumerable<StageCardModel> CardModels { get; set; }
}

public class StageCardModel
{
    public Guid StageCardId { get; set; }
    public Guid HrmUserId { get; set; }
    public Guid StageId { get; set; }
    public List<CandidateEventModel> Events { get; set; }
}

public class BoardView
{
    public Guid BoardId { get; set; }
    [Required] public List<StageCreationModel> Stages { get; set; } = null!;
}

public class CandidateInfoModel
{
    public Guid HrmUserId { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string Surname { get; set; } = null!;
    public string Email { get; set; }
    public string TgLogn { get; set; }
    public string? SecondName { get; set; }
    public double WorkYears { get; set; }
    [Required] public CandidateEventModel Event { get; set; } = null!;
}

public class CandidateEventModel
{
    public Guid EventId { get; set; }
    [Required] public string Title { get; set; }
    public DateTimeOffset Date { get; set; }
    [Required] public string AccountableFio { get; set; } = null!;
}

public class AccountableModel
{
    public Guid AccountableId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string? LastName { get; set; }
}

public class CommentModel
{
    public Guid StageCardId { get; set; }
    public string Comment { get; set; }
}
// public class CreateEventRequest
// {
//     public Guid AccountableId { get; set; }
//     public DateTimeOffset Date { get; set; }
//     [Required] public string EventType { get; set; } = null!;
// }
//
// public class CreateStageRequest
// {
//     public Guid? StageId { get; set; }
//     [Required] public string Title { get; set; }
//     public int Position { get; set; }
//     public Guid BoardId { get; set; }
// }