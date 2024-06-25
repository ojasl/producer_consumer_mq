using ProducerConsumer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Models.Model
{
    public class ConsumeMessageResponse : BaseResponse
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }
}
