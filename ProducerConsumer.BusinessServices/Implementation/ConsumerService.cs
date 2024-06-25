using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MSMQ.Messaging;
using ProducerConsumer.BusinessServices.Interface;
using ProducerConsumer.Data.Interface;
using ProducerConsumer.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.BusinessServices.Implementation
{
    public class ConsumerService : IConsumerService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IProducerConsumerDAL _producerConsumerDAL;

        public ConsumerService(IMessageQueue messageQueue, IProducerConsumerDAL producerConsumerDAL)
        {
            _messageQueue = messageQueue;
            _producerConsumerDAL = producerConsumerDAL;
        }

        public async Task<ConsumeMessageResponse> ConsumeMessage()
        {
            try
            {
                var messages = _messageQueue.GetAllMessages();

                var successCount = 0;
                var errorCount = 0;

                foreach (var message in messages)
                {
                    try
                    {
                        var messageString = _messageQueue.Receive().Body.ToString();
                        successCount++;

                        await _producerConsumerDAL.UpdateMessageStatus(messageString, "Read");
                    }
                    catch
                    {
                        errorCount++;

                        await _producerConsumerDAL.UpdateMessageStatus(message.Body.ToString(), "Error");
                    }
                }

                return new ConsumeMessageResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Status = true,
                    Message = "Message consumed successfully",
                    SuccessCount = successCount,
                    ErrorCount = errorCount
                };
            }
            catch
            {
                return new ConsumeMessageResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = false,
                    Message = "Failed to consume message"
                };
            }
        }
    }
}
