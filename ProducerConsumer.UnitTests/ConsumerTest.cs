using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using MSMQ.Messaging;
using ProducerConsumer.BusinessServices.Implementation;
using ProducerConsumer.BusinessServices.Interface;
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
    public class ConsumerTest
    {
        [Fact]
        public async Task ConsumeMessage_Success()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockGenericRepository = new Mock<IGenericRepository<MessageList>>();

            mockConfiguration.Setup(x => x["MessageQueuePath"]).Returns(".\\Private$\\ConsumerSuccessTestQueue");

            var producerConsumerDAL = new ProducerConsumerDAL(mockUnitOfWork.Object);
            var messageQueueWrapper = new MessageQueueWrapper(mockConfiguration.Object);
            var consumerService = new ConsumerService(messageQueueWrapper, producerConsumerDAL);
            var consumerController = new ConsumerController(consumerService);

            var producerService = new ProducerService(mockConfiguration.Object, producerConsumerDAL);
            var producerController = new ProducerController(producerService);

            mockGenericRepository.Setup(x => x.Insert(It.IsAny<MessageList>())).Verifiable();
            mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockGenericRepository.Object);

            var messageEntity = new MessageList
            {
                Id = 1,
                Message = "Hi",
                Status = "Not Read"
            };

            mockGenericRepository.Setup(x => x.Get(x => x.Message == It.IsAny<string>())).ReturnsAsync(messageEntity);
            mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockGenericRepository.Object);

            mockGenericRepository.Setup(x => x.Update(It.IsAny<MessageList>())).Verifiable();
            mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockGenericRepository.Object);

            //Add Message to Queue
            await producerController.ProduceMessage(new ProduceMessageRequest { Message = "Hi" });

            //Act
            var result = await consumerController.ConsumeMessage();

            //Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseWrapper = Assert.IsType<ResponseWrapper<ConsumeMessageResponse>>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, responseWrapper.StatusCode);
            Assert.True(responseWrapper.Status);
            Assert.Equal("Message consumed successfully", responseWrapper.Message);
            Assert.Equal(1, responseWrapper.Result.SuccessCount);
            Assert.Equal(0, responseWrapper.Result.ErrorCount);

        }

        [Fact]
        public async Task ConsumeMessage_ErrorCountGreaterThanZero()
        {
            // Arrange
            var mockMessageQueue = new Mock<IMessageQueue>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockGenericRepository = new Mock<IGenericRepository<MessageList>>();

            // Create dummy messages
            var messages = new Message[] { new Message() { Body = "Message 1"}, new Message() { Body = "Message 2" }, new Message() { Body = "Message 3" } };

            mockMessageQueue.Setup(mq => mq.GetAllMessages()).Returns(messages);


            mockGenericRepository.Setup(x => x.Get(x => x.Message == It.IsAny<string>())).ReturnsAsync(null as MessageList);
            mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockGenericRepository.Object);

            ProducerConsumerDAL producerConsumerDAL = new ProducerConsumerDAL(mockUnitOfWork.Object);

            var consumerService = new ConsumerService(mockMessageQueue.Object, producerConsumerDAL);

            var consumerController = new ConsumerController(consumerService);

            // Act
            var result = await consumerController.ConsumeMessage();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseWrapper = Assert.IsType<ResponseWrapper<ConsumeMessageResponse>>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, responseWrapper.StatusCode);
            Assert.True(responseWrapper.Status);
            Assert.Equal("Message consumed successfully", responseWrapper.Message);
            Assert.Equal(0, responseWrapper.Result.SuccessCount);
            Assert.Equal(3, responseWrapper.Result.ErrorCount);
        }
    }
}
