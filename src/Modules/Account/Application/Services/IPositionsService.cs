using Astrum.Account.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services
{
    public interface IPositionsService
    {
        public Task<Result<List<PositionForm>>> GetPositions(CancellationToken cancellationToken = default);
        public Task<Result<List<PositionForm>>> GetPositionsByName(string value, CancellationToken cancellationToken = default);
        public Task<Result<PositionForm>> GetPositionById(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<PositionForm>> UpdatePosition(Guid id, PositionForm positionForm, CancellationToken cancellationToken = default);
        public Task<Result<PositionForm>> CreatePosition(PositionForm positionForm, CancellationToken cancellationToken = default);
        public Task<Result<PositionForm>> DeletePosition(Guid id, CancellationToken cancellationToken = default);
        public Task<bool> PositionAlreadyExists(string positionName, CancellationToken cancellationToken = default);
    }
}
