using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Inventory.Application.Commands
{
    public class UpdateInventoryItemCommandHadler : CommandResultHandler<UpdateInventoryItemCommand, InventoryItemView>
    {
        private readonly IInventoryItemsService _service;
        private readonly IMapper _mapper;
        public UpdateInventoryItemCommandHadler(IInventoryItemsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async override Task<Result<InventoryItemView>> Handle(UpdateInventoryItemCommand command, CancellationToken cancellationToken = default)
        {
            var inventoryItem = await _service.GetInventoryItemById(command.InventoryItemId);
            if (inventoryItem == null)
            {
                return Result.NotFound("Предмет не найден");
            }
            var response = await _service.Update(command.InventoryItemId, command.InventoryItem, cancellationToken);
            return Result.Success(response);
        }
    }
}
