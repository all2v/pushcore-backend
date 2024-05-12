//using System.Linq.Dynamic.Core;
//using Astrum.Inventory.Application.Models;
//using Astrum.Inventory.Application.Services;
//using Astrum.Inventory.Domain.Aggregates;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace Astrum.Inventory.Tests.InventoryItems.Get
//{
//    public class GetFilteringInventoryItemsTests
//    {
//        private string model = "Model2";
//        private Guid userId = Guid.Parse("b2197d1f-8e5d-4f6f-a5cf-f260ed38fdcb");
//        private StatusDTO statusDto = StatusDTO.InWarehouse;
//        private Status status = Status.InWarehouse;
//        private string templateName = "TemplateName2";

//        [Fact]
//        public async Task GetFilteringItems_ShouldBeFullFilteringItems()
//        {
//            // Arrange
//            var baseValues = new List<InventoryItemView>()
//            {
//                new InventoryItemView
//                {
//                    Model = model,
//                    UserId = userId,
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = templateName,
//                    Status = statusDto,
//                    SerialNumber = "SerialNumber1",
//                    Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = TypeDTO.String,
//                            Value = "Value3",
//                            IsCustomField = true
//                        },
//                    }
//                },
//                new InventoryItemView
//                {
//                    Model = model,
//                    UserId = userId,
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = templateName,
//                    Status = statusDto,
//                    SerialNumber = "SerialNumber2",
//                    Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = TypeDTO.String,
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
//                    TemplateName = "TemplateName5",
//                    Status = StatusDTO.InUsing,
//                    SerialNumber = "SerialNumber2",
//                    Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = TypeDTO.String,
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
//                    TemplateName = "TemplateName5",
//                    Status = StatusDTO.InUsing,
//                    SerialNumber = "SerialNumber4",
//                    Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = TypeDTO.String,
//                            Value = "Value3",
//                            IsCustomField = true
//                        },
//                    }
//                },
//            };
//            var filteringItems = new List<InventoryItemView>()
//            {
//                new InventoryItemView
//                {
//                    Model = model,
//                    UserId = userId,
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = templateName,
//                    Status = statusDto,
//                    SerialNumber = "SerialNumber1",
//                    Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = TypeDTO.String,
//                            Value = "Value3",
//                            IsCustomField = true
//                        },
//                    }
//                },
//                new InventoryItemView
//                {
//                    Model = model,
//                    UserId = userId,
//                    DateCreated = DateTime.Today,
//                    IsPublic = true,
//                    State = 100,
//                    TemplateName = templateName,
//                    Status = statusDto,
//                    SerialNumber = "SerialNumber2",
//                    Characteristics = new List<CharacteristicView>()
//                    {
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic1",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic2",
//                            Type = TypeDTO.String,
//                            Value = "Value2",
//                            IsCustomField = true
//                        },
//                        new CharacteristicView
//                        {
//                            Name = "Characteristic3",
//                            Type = TypeDTO.String,
//                            Value = "Value3",
//                            IsCustomField = true
//                        },
//                    }
//                },
//            };
//            var stub = Mock.Of<IInventoryItemsService>(c => c.GetFilteringInventoryItems
//            (templateName, model, status, userId, null, null, It.IsAny<CancellationToken>()) == Task.FromResult(baseValues));
//            //Act
//            var result = await stub.GetFilteringInventoryItems(templateName, model, status, userId, null, null);

//            //Assert
//            result.Should().Contain(filteringItems);
//            result.Count.Should().Be(2);
//            result.ForEach(x => x.Characteristics.Count.Should().Be(3));
//            result.Should().BeOfType<List<InventoryItemView>>();
//        }
//    }
//}
