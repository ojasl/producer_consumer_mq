using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Data.Interface
{
    public interface IProducerConsumerDAL
    {
        Task AddMessage(string message);
        Task UpdateMessageStatus(string message, string status);
    }
}
