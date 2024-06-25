using ProducerConsumer.Data.Entities;
using ProducerConsumer.Data.Interface;
using ProducerConsumer.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Data.Implementation
{
    public class ProducerConsumerDAL : IProducerConsumerDAL
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProducerConsumerDAL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddMessage(string message)
        {
            var messageEntity = new MessageList
            {
                Message = message,
                Status = "Not Read"
                
            };
            await _unitOfWork.MessageRepository.Insert(messageEntity);
        }

        public async Task UpdateMessageStatus(string message, string status)
        {
            var messageEntity = await _unitOfWork.MessageRepository.Get( x => x.Message == message);
            
            if(messageEntity != null)
            {
                messageEntity.Status = status;
                await _unitOfWork.MessageRepository.Update(messageEntity);
            }
        }
    }
}
