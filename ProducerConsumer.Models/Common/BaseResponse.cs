using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProducerConsumer.Models.Common
{
    public class BaseResponse
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        [JsonIgnore]
        public string Message { get; set; } = string.Empty;
    }
}
