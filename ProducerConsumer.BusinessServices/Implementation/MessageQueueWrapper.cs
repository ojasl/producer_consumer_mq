using Microsoft.Extensions.Configuration;
using MSMQ.Messaging;
using ProducerConsumer.BusinessServices.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.BusinessServices.Implementation
{
    public class MessageQueueWrapper : IMessageQueue
    {
        private readonly IConfiguration _configuration;

        private readonly MessageQueue _messageQueue;

        public MessageQueueWrapper(IConfiguration configuration)
        {
            _configuration = configuration;

            var messageQueuePath = _configuration["MessageQueuePath"] ?? string.Empty;

            _messageQueue = new MessageQueue(messageQueuePath);

            _messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
        }

        public Message[] GetAllMessages()
        {
            return _messageQueue.GetAllMessages();
        }

        public Message Receive()
        {
            return _messageQueue.Receive();
        }
    }
}
