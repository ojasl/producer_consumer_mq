using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MSMQ.Messaging;
using ProducerConsumer.BusinessServices.Interface;
using ProducerConsumer.Data.Implementation;
using ProducerConsumer.Data.Interface;
using ProducerConsumer.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.BusinessServices.Implementation
{
    public class ProducerService : IProducerService
    {
        private readonly string _messageQueuePath;
        private readonly IProducerConsumerDAL _producerConsumerDAL;

        public ProducerService(IConfiguration configuration, IProducerConsumerDAL producerConsumerDAL)
        {
            var _configuration = configuration;
            
            _messageQueuePath = _configuration["MessageQueuePath"] ?? string.Empty;

            if(!MessageQueue.Exists(_messageQueuePath))
            {
                MessageQueue.Create(_messageQueuePath);
            }

            _producerConsumerDAL = producerConsumerDAL;
        }

        public async Task<ProduceMessageResponse> ProduceMessage(ProduceMessageRequest produceMessageRequest)
        {
            try
            {
                var queue = new MessageQueue(_messageQueuePath);

                queue.Send(produceMessageRequest.Message);

                await _producerConsumerDAL.AddMessage(produceMessageRequest.Message);

                return new ProduceMessageResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Status = true,
                    Message = "Message produced successfully",
                    TotalUnreadMessageCount = queue.GetAllMessages().Length
                };
            }
            catch
            {
                return new ProduceMessageResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = false,
                    Message = "Failed to produce message"
                };
            }
            
        }
    }
}
