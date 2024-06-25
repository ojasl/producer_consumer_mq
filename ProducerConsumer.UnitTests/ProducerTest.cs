using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using MSMQ.Messaging;
using ProducerConsumer.BusinessServices.Implementation;
using ProducerConsumer.Data.Entities;
using ProducerConsumer.Data.Implementation;
using ProducerConsumer.Data.IRepository;
using ProducerConsumer.Models.Common;
using ProducerConsumer.Models.Model;
using ProducerConsumer.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.UnitTests
{
    public class ProducerTest
    {
        [Fact]
        public async Task ProduceMessage_Success()
        {
            // Arrange
            var produceMessageRequest = new ProduceMessageRequest
            {
                Message = "Hi"
            };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockGenericRepository = new Mock<IGenericRepository<MessageList>>();

            mockConfiguration.Setup(x => x["MessageQueuePath"]).Returns(".\\Private$\\ProducerTestQueue");

            var producerConsumerDAL = new ProducerConsumerDAL(mockUnitOfWork.Object);
            var producerService = new ProducerService(mockConfiguration.Object, producerConsumerDAL);
            var producerController = new ProducerController(producerService);

            mockGenericRepository.Setup(x => x.Insert(It.IsAny<MessageList>())).Verifiable();
            mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockGenericRepository.Object);


            // Act
            var result = await producerController.ProduceMessage(produceMessageRequest);

            var messageQueueWrapper = new MessageQueueWrapper(mockConfiguration.Object);
            var consumerService = new ConsumerService(messageQueueWrapper, producerConsumerDAL);
            var consumerController = new ConsumerController(consumerService);

            mockGenericRepository.Setup(x => x.Get(x => x.Message == It.IsAny<string>())).ReturnsAsync(null as MessageList);
            mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockGenericRepository.Object);

            await consumerController.ConsumeMessage();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseWrapper = Assert.IsType<ResponseWrapper<ProduceMessageResponse>>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, responseWrapper.StatusCode);
            Assert.True(responseWrapper.Status);
            Assert.Equal("Message produced successfully", responseWrapper.Message);
            Assert.Equal(1, responseWrapper.Result.TotalUnreadMessageCount);


            
        }
    }
}
