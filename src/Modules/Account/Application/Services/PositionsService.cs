using Astrum.Account.Application.Repositories;
using Astrum.Account.Domain.Aggregates;
using Astrum.Account.Specifications;
using Astrum.Account.ViewModels;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Account.Services
{
    public class PositionsService : IPositionsService
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _repository;

        public PositionsService(IMapper mapper, IPositionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<List<PositionForm>>> GetPositions(CancellationToken cancellationToken = default)
        {
            var spec = new GetPositionsSpec();
            var positions = await _repository.ListAsync(spec, cancellationToken);
            return Result.Success(_mapper.Map<List<PositionForm>>(positions));
        }

        public async Task<Result<List<PositionForm>>> GetPositionsByName(string value, CancellationToken cancellationToken = default)
        {
            var spec = new GetPositionByNameSpec(value);
            var positions = await _repository.ListAsync(spec, cancellationToken);
            return Result.Success(_mapper.Map<List<PositionForm>>(positions));
        }

        public async Task<Result<PositionForm>> GetPositionById(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetPositionByIdSpec(id);
            var position = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (position == null)
            {
                return Result.NotFound("Должность не найдена.");
            }
            return Result.Success(_mapper.Map<PositionForm>(position));
        }

        public async Task<Result<PositionForm>> UpdatePosition(Guid id, PositionForm positionForm, CancellationToken cancellationToken = default)
        {
            var position = _mapper.Map<Position>(positionForm);
            var spec = new GetPositionByIdSpec(id);
            var dbPosition = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (position == null)
            {
                return Result.NotFound("Должность не найдена.");
            }
            dbPosition.Name = position.Name;
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении должности.");
            }
            return Result.Success(_mapper.Map<PositionForm>(dbPosition));
        }

        public async Task<Result<PositionForm>> CreatePosition(PositionForm positionForm, CancellationToken cancellationToken = default)
        {
            var position = _mapper.Map<Position>(positionForm);
            position = await _repository.AddAsync(position);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создани должности.");
            }
            return Result.Success(_mapper.Map<PositionForm>(position));
        }

        public async Task<Result<PositionForm>> DeletePosition(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetPositionByIdSpec(id);
            var position = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (position == null)
            {
                return Result.NotFound("Должность не найдена.");
            }
            try
            {
                await _repository.DeleteAsync(position);
                await _repository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении должности.");
            }
            
            return Result.Success(_mapper.Map<PositionForm>(position));
        }

        public async Task<bool> PositionAlreadyExists(string positionName, CancellationToken cancellationToken = default)
        {
            var spec = new GetPositionsSpec();
            var positions = await _repository.ListAsync(spec, cancellationToken);
            return positions.Any(position => position.Name.ToLower().Trim() == positionName.ToLower().Trim());
        }
    }
}
