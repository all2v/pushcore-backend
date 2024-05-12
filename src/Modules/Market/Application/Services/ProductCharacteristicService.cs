namespace Astrum.Market.Services;

//public class ProductCharacteristicService : IProductCharacteristicService
//{
//    private readonly IRepository<ProductCharacteristic> _repository;

//    public ProductCharacteristicService(IRepository<ProductCharacteristic> repository)
//    {
//        _repository = repository;
//    }

//    public async Task AddCharacteristic(Guid productId, ProductCharacteristicForm characteristic)
//    {
//        var ch = new ProductCharacteristic
//        {
//            Id = (Guid) characteristic.Id,
//            ProductId = productId,
//            Name = characteristic.Name,
//            Description = characteristic.Description,
//            Value = characteristic.Value
//        };
//        await _repository.AddAsync(ch);
//        await _repository.UnitOfWork.SaveChangesAsync();
//    }

//    public async Task UpdateCharacteristic(Guid id, ProductCharacteristicForm characteristic)
//    {
//        var ch = await _repository.FindAsync(id);
//        ch.Name = characteristic.Name ?? ch.Name;
//        ch.Description = characteristic.Description ?? ch.Description;
//        ch.Value = characteristic.Value ?? ch.Value;
//        await _repository.UnitOfWork.SaveChangesAsync();
//    }

//    public async Task RemoveCharacteristic(Guid id)
//    {
//        var ch = await _repository.FindAsync(id);
//        await _repository.RemoveAsync(ch);
//        await _repository.UnitOfWork.SaveChangesAsync();
//    }
//}