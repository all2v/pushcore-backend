using Astrum.Market.ViewModels;

namespace Astrum.Market.Services;

public interface IProductCharacteristicService
{
    Task AddCharacteristic(Guid productId, ProductCharacteristicForm characteristic);
    Task UpdateCharacteristic(Guid id, ProductCharacteristicForm characteristic);
    Task RemoveCharacteristic(Guid id);
}