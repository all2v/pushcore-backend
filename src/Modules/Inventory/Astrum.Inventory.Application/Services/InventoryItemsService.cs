using Astrum.Account.Services;
using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Models.ViewModels;
using Astrum.Inventory.Domain.Aggregates;
using Astrum.Inventory.Domain.Specifications;
using Astrum.Inventory.DomainServices.Repositories;
using Astrum.News.Aggregates;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using MassTransit;
using Minio.DataModel;
using Newtonsoft.Json;

namespace Astrum.Inventory.Application.Services
{
    public class InventoryItemsService : IInventoryItemsService
    {
        private readonly IInventoryItemsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserProfileService _userProfileServices;
        private readonly IFileStorage _fileStorage;

        public InventoryItemsService(IInventoryItemsRepository repository, IMapper mapper, IUserProfileService userProfileService,
            IFileStorage fileStorage)
        {
            _repository = repository;
            _mapper = mapper;
            _userProfileServices = userProfileService;
            _fileStorage = fileStorage;
        }

        public async Task<Result<List<InventoryItemView>>> GetInventoryItems(CancellationToken cancellationToken = default)
        {
            var spec = new GetInventoryItemsSpec();
            var inventoryItems = await _repository.ListAsync(spec, cancellationToken);
            var itemViews = _mapper.Map<List<InventoryItemView>>(inventoryItems);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            foreach (var item in itemViews.Where(item => item.UserId != null))
            {
                var user = _mapper.Map<UserInventory>(users.Data.FirstOrDefault(user => user.UserId == item.UserId));
                item.User = user;
            }
            foreach (var item in itemViews.Where(item => item.PictureId != null))
            {
                item.LinkImage = await _fileStorage.GetFileUrl(item.PictureId.Value);
            }
            return itemViews;
        }

        public async Task<Result<InventoryItemsPaginationView>> GetFilteringInventoryItems(string[]? templates, string? predicate,
            Status[]? statuses, Guid? userId, int? startIndex = 1, int? count = 15, CancellationToken cancellationToken = default)
        {
            if (startIndex < 0) startIndex = 0;
            if (count < 0) count = 5;

            var spec = new GetFilteringInventoryItems(templates, predicate, statuses, userId, startIndex, count);
            var inventoryItems = await _repository.ListAsync(spec, cancellationToken);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();

            if (inventoryItems.Count == 0)
            {
                return Result.Success(new InventoryItemsPaginationView
                {
                    InventoryItems = new(),
                    Index = 0,
                    NextExist = false
                });
            }
            var lastIndex = inventoryItems[inventoryItems.Count - 1].Index;
            var nextExistspec = new CheckNextSpec(lastIndex);
            var nextExist = await _repository.AnyAsync(nextExistspec);

            var itemViews = _mapper.Map<List<InventoryItemView>>(inventoryItems);
            foreach (var item in itemViews.Where(item => item.UserId != null))
            {
                var user = _mapper.Map<UserInventory>(users.Data.FirstOrDefault(user => user.UserId == item.UserId));
                item.User = user;
            }
            foreach (var item in itemViews.Where(item => item.PictureId != null))
            {
                item.LinkImage = await _fileStorage.GetFileUrl(item.PictureId.Value);
            }

            var result = new InventoryItemsPaginationView
            {
                InventoryItems = itemViews,
                Index = lastIndex,
                NextExist = nextExist
            };
            return Result.Success(result);
        }

        public async Task<Result<InventoryItemView>> GetInventoryItemById(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetInventoryItemById(id);
            var inventoryItem = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (inventoryItem == null)
            {
                return Result.NotFound("Товар не найден по id.");
            }
            var item = _mapper.Map<InventoryItemView>(inventoryItem);
            if (item.UserId != null)
            {
                var userProfileSummary = await _userProfileServices.GetUserProfileSummaryAsync((Guid)item.UserId);
                var user = _mapper.Map<UserInventory>(userProfileSummary.Data);
                item.User = user;
            }
            if (item.PictureId != null)
            {
                item.LinkImage = await _fileStorage.GetFileUrl(item.PictureId.Value);
            }
            return Result.Success(item);
        }

        public async Task<Result<List<InventoryItemView>>> GetInventoryItemByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            var spec = new GetInventoryItemsByUserId(userId);
            var items = _mapper.Map<List<InventoryItemView>>(await _repository.ListAsync(spec, cancellationToken));
            var user = _mapper.Map<UserInventory>(await _userProfileServices.GetUserProfileSummaryAsync(userId));
            foreach (var item in items)
            {
                item.User = user;
            }
            foreach (var item in items.Where(item => item.PictureId != null))
            {
                item.LinkImage = await _fileStorage.GetFileUrl(item.PictureId.Value);
            }
            return items;
        }

        public async Task<Result<InventoryItemView>> Create(InventoryItemCreateRequest inventoryItemCreate, CancellationToken cancellationToken = default)
        {
            var inventory = _mapper.Map<InventoryItem>(inventoryItemCreate);
            if (inventoryItemCreate.Image != null)
            {
                if (await _fileStorage.FileExists(inventoryItemCreate.Image.Id) == false)
                {
                    var res = await _fileStorage.UploadFile(inventoryItemCreate.Image, cancellationToken);
                    if (res != null && res.Success)
                    {
                        inventory.PictureId = res.UploadedFileId;
                    }
                }
                else
                {
                    inventory.PictureId = inventoryItemCreate.Image.Id;
                }
            }
            inventory = await _repository.AddAsync(inventory, cancellationToken);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создании товара.");
            }
            var inventoryItemView = _mapper.Map<InventoryItemView>(inventory);
            if (inventoryItemView.UserId != null)
            {
                var userProfileSummary = await _userProfileServices.GetUserProfileSummaryAsync((Guid)inventoryItemView.UserId);
                var user = _mapper.Map<UserInventory>(userProfileSummary.Data);
                inventoryItemView.User = user;
            }
            if (inventory.PictureId.HasValue)
            {
                inventoryItemView.LinkImage = await _fileStorage.GetFileUrl(inventory.PictureId.Value);
            }
            return Result.Success(inventoryItemView);
        }

        public async Task<Result<InventoryItemView>> Update(Guid id, InventoryItemUpdateRequest inventoryItemUpdate,
            CancellationToken cancellationToken = default)
        {
            var spec = new GetInventoryItemById(id);
            var inventoryDb = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (inventoryDb == null)
                return Result.NotFound("Товар не найден по id.");
            if (inventoryItemUpdate.Image != null)
            {
                if (await _fileStorage.FileExists(inventoryItemUpdate.Image.Id) == false)
                {
                    var res = await _fileStorage.UploadFile(inventoryItemUpdate.Image, cancellationToken);
                    if (res != null && res.Success)
                    {
                        inventoryDb.PictureId = res.UploadedFileId;
                    }
                }
                else
                {
                    inventoryDb.PictureId = inventoryItemUpdate.Image.Id;
                }
            }
            _mapper.Map(inventoryItemUpdate, inventoryDb);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении товара.");
            }
            var item = _mapper.Map<InventoryItemView>(inventoryDb);
            if (item.UserId != null)
            {
                var userProfileSummary = await _userProfileServices.GetUserProfileSummaryAsync((Guid)item.UserId);
                var user = _mapper.Map<UserInventory>(userProfileSummary.Data);
                item.User = user;
            }
            if (inventoryDb.PictureId.HasValue)
            {
                item.LinkImage = await _fileStorage.GetFileUrl(inventoryDb.PictureId.Value);
            }
            return Result.Success(item);
        }

        public async Task<Result<InventoryItemView>> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetInventoryItemById(id);
            var item = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (item == null)
                return Result.NotFound("Товар не найден по id.");
            try
            {
                await _repository.DeleteAsync(item, cancellationToken);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении товара.");
            }
            var inventoryItemView = _mapper.Map<InventoryItemView>(item);
            return Result.Success(inventoryItemView);
        }
    }
}
