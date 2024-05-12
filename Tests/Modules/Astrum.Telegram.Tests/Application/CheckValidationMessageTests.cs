using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Telegram.Services;
using Moq;
using Telegram.Bot;
using Xunit;

namespace Astrum.Telegram.Tests.Application
{
    public class CheckValidationMessageTests
    {
        private Mock<ITelegramBotClient> mockTelegram = new Mock<ITelegramBotClient>();
        private Mock<ISubscrubitionManager> mockSubscrubition = new Mock<ISubscrubitionManager>();
        [Fact]
        public async Task CheckValidationMessage_WhenLengthArrayNotTwo_ReturnFalse()
        {
            //Arrange
            MessageService messageService =
                new MessageService(mockTelegram.Object, mockSubscrubition.Object);
            string text = "test test test";

            //Act
            var result = messageService.CheckValidationMessage(text);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckValidationMessage_WhenGuidNot36Symbols_ReturnFalse()
        {
            //Arrange
            MessageService messageService =
                new MessageService(mockTelegram.Object, mockSubscrubition.Object);
            string text = "test test";

            //Act
            var result = messageService.CheckValidationMessage(text);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckValidationMessage_WhenAllValid_ReturnTrue()
        {
            //Arrange
            MessageService messageService =
                new MessageService(mockTelegram.Object, mockSubscrubition.Object);
            string text = "test 1c2b4bc3-e03f-4328-b4a8-dc9be683d8c6";

            //Act
            var result = messageService.CheckValidationMessage(text);

            //Assert
            Assert.True(result);
        }
    }
}
