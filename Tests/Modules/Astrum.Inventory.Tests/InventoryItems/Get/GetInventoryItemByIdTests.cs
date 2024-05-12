//using Astrum.Inventory.Application.Models.ViewModels;
//using Astrum.Inventory.Application.Services;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace Astrum.Inventory.Tests.InventoryItems.Get
//{
//    public class GetInventoryItemByIdTests
//    {
//        [Fact]
//        public async Task GetInventoryItemById_ShouldReturnItemById()
//        {
//            //Arrange
//            var id = Guid.Parse("7b88ddc2-0d57-424e-b504-11c77863cce7");
//            var inventoryItem = new InventoryItemView
//            {
//                Model = "Model1",
//                UserId = id,
//                DateCreated = DateTime.Today,
//                IsPublic = true,
//                State = 100,
//                TemplateName = "TemplateName1",
//                Status = Application.Models.StatusDTO.InWarehouse,
//                SerialNumber = "SerialNumber1",
//                Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = Application.Models.TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = Application.Models.TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = Application.Models.TypeDTO.String,
//                            Value = "Value3",
//                            IsCustomField = true
//                        },
//                    }

//            };

//            var stub = Mock.Of<IInventoryItemsService>(x => x.GetInventoryItemById(id, It.IsAny<CancellationToken>()) == Task.FromResult(inventoryItem));
            
//            //Act
//            var result = await stub.GetInventoryItemById(id);

//            //Assert
//            result.Should().Be(inventoryItem);
//            result.Should().NotBeNull();
//            result.Should().BeOfType<InventoryItemView>();
//            result.Characteristics.Count.Should().Be(3);
//        }
//    }
//}
