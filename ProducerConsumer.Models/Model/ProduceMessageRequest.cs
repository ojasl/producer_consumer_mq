using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Models.Model
{
    public class ProduceMessageRequest
    {
        [Required]
        public string Message { get; set; }
    }
}
