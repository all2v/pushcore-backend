//using Astrum.Identity.Managers;
//using Astrum.News.Repositories;
//using Astrum.Telegram.Application.ViewModels.Results;
//using Astrum.Telegram.Services;
//using Astrum.Tests;
//using Moq;
//using Telegram.Bot;
//using Xunit;

//namespace Astrum.Telegram.Tests.Application
//{
//    public class SubscribeCompanyChatTests
//    {
//        //private readonly ISubscrubitionManager _manager;

//        //public SubscribeCompanyChatTests(ISubscrubitionManager manager)
//        //{
//        //    _manager = manager;
//        //}

//        [Fact]
//        public async Task SubscribeCompanyChat_WhenChatIdMathes_ReturnError()
//        {
//            //Arrange
//            string userId = "test";
//            string securityHash = "test";
//            long chatId = 4337;
//            string chatName = "test";
//            long userChatId = 1;

//            ActionResult actionResult = new ActionResult()
//            {
//                Errors = new List<string>()
//                {
//                    "Ошибка: чат уже добавлен!"
//                }
//            };

//            var stub = Mock.Of<ISubscrubitionManager>
//                (manager => manager.SubscribeCompanyChat
//                (userId, securityHash, chatId, chatName, userChatId) == Task.FromResult(actionResult));

//            //Act
//            var result = stub.SubscribeCompanyChat(userId, securityHash, chatId, chatName, userChatId);

//            //Assert
//            Assert.Equal("Ошибка: чат уже добавлен!", actionResult.Errors.First());
//        }

//        [Fact]
//        public async Task SubscribeCompanyChat_WhenUserNotExists_ReturnError()
//        {

//        }

//        [Fact]
//        public async Task SubscribeCompanyChat_WhenSecurityHashNotValid_ReturnError()
//        {

//        }

//        [Fact]
//        public async Task SubscribeCompanyChat_WhenAllValid_ReturnSucceed()
//        {

//        }
//    }
//}
