using ProducerConsumer.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.BusinessServices.Interface
{
    public interface IConsumerService
    {
        Task<ConsumeMessageResponse> ConsumeMessage();
    }
}
