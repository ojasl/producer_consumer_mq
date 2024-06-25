using ProducerConsumer.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.BusinessServices.Interface
{
    public interface IProducerService
    {
        Task<ProduceMessageResponse> ProduceMessage(ProduceMessageRequest produceMessageRequest);
    }
}
