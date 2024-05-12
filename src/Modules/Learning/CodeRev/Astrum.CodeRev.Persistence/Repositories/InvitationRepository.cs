using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.CodeRev.Persistence.Repositories;

public class InvitationRepository : EFRepository<Invitation, Guid, CodeRevDbContext>,
    IInvitationRepository
{
    public InvitationRepository(CodeRevDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}