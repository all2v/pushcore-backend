using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public interface IAnswerOptionRepository: IEntityRepository<QuestionAnswerOption, Guid>
{
    
}