using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Inventory.Application.Commands
{
    public class DeleteInventoryItemCommandHandler : CommandResultHandler<DeleteInventoryItemCommand, InventoryItemView>
    {
        private readonly IInventoryItemsService _service;
        private readonly IMapper _mapper;
        public DeleteInventoryItemCommandHandler(IInventoryItemsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async override Task<Result<InventoryItemView>> Handle(DeleteInventoryItemCommand command, CancellationToken cancellationToken = default)
        {
            var inventoryItem = await _service.GetInventoryItemById(command.InventoryItemId);
            if (inventoryItem == null)
            {
                return Result.NotFound("Предмет не найден");
            }
            var response = await _service.Delete(command.InventoryItemId, cancellationToken);
            return Result.Success(response);
        }
    }
}
