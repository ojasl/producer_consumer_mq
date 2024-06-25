using ProducerConsumer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Models.Model
{
    public class ProduceMessageResponse : BaseResponse
    {
        public int TotalUnreadMessageCount { get; set; }
    }
}
