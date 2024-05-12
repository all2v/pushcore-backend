//using Astrum.Inventory.Application.Models.ViewModels;
//using Astrum.Inventory.Application.Services;
//using Moq;
//using Xunit;
//using FluentAssertions;

//namespace Astrum.Inventory.Tests.InventoryItems.Get
//{
//    public class GetInventoryItemsTests
//    {
//        [Fact]
//        public async Task GetInventoryItems_ShouldReturnItems()
//        {
//            // Arrange
//            var values = new List<InventoryItemView>()
//            {
//                new InventoryItemView
//                {
//                    Model = "Model1",
//                    UserId = Guid.NewGuid(),
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = "TemplateName1",
//                    Status = Application.Models.StatusDTO.InWarehouse,
//                    SerialNumber = "SerialNumber1",
//                    Characteristics = new List<CharacteristicView>()
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
//                },
//                new InventoryItemView
//                {
//                    Model = "Model2",
//                    UserId = Guid.NewGuid(),
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = "TemplateName2",
//                    Status = Application.Models.StatusDTO.InWarehouse,
//                    SerialNumber = "SerialNumber2",
//                    Characteristics = new List<CharacteristicView>()
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
//                },
//                new InventoryItemView
//                {
//                    Model = "Model3",
//                    UserId = Guid.NewGuid(),
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = "TemplateName3",
//                    Status = Application.Models.StatusDTO.InWarehouse,
//                    SerialNumber = "SerialNumber3",
//                    Characteristics = new List<CharacteristicView>()
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
//                },
//                new InventoryItemView
//                {
//                    Model = "Model4",
//                    UserId = Guid.NewGuid(),
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = "TemplateName4",
//                    Status = Application.Models.StatusDTO.InWarehouse,
//                    SerialNumber = "SerialNumber4",
//                    Characteristics = new List<CharacteristicView>()
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
//                },
//            };
//            var stub = Mock.Of<IInventoryItemsService>(x => x.GetInventoryItems(It.IsAny<CancellationToken>()) == Task.FromResult(values));
            
//            // Act
//            var result = await stub.GetInventoryItems();
            
//            // Assert
//            result.Should().Contain(values);
//            result.Count.Should().Be(4);
//            result.ForEach(x => x.Characteristics.Count.Should().Be(3));
//            result.Should().BeOfType<List<InventoryItemView>>();
//        }
//    }
//}
