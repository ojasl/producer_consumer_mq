using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Models.Common
{
    public class ResponseWrapper<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
        public T Result { get; set; } = default!;
    }


    public class ErrorDetail
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
    }
}
