using Microsoft.AspNetCore.Mvc;
using ProducerConsumer.BusinessServices.Interface;
using ProducerConsumer.Models.Common;
using ProducerConsumer.Models.Model;

namespace ProducerConsumer.WebAPI.Controllers
{
    public class ProducerController : ApiBaseController
    {
        private readonly IProducerService _producerService;

        public ProducerController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> ProduceMessage(ProduceMessageRequest produceMessageRequest)
        {
            var produceMessageResponse = await _producerService.ProduceMessage(produceMessageRequest);

            if (produceMessageResponse.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(new ResponseWrapper<ProduceMessageResponse>()
                {
                    StatusCode = produceMessageResponse.StatusCode,
                    Status = produceMessageResponse.Status,
                    Message = produceMessageResponse.Message,
                    Result = new ProduceMessageResponse()
                    {
                        TotalUnreadMessageCount = produceMessageResponse.TotalUnreadMessageCount
                    }
                });
            }
            
            return StatusCode(produceMessageResponse.StatusCode, new ErrorDetail()
            {
                StatusCode = produceMessageResponse.StatusCode,
                Status = produceMessageResponse.Status,
                Message = produceMessageResponse.Message
            });
        }
    }
}
