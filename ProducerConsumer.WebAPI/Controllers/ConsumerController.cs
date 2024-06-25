using Microsoft.AspNetCore.Mvc;
using ProducerConsumer.BusinessServices.Interface;
using ProducerConsumer.Models.Common;
using ProducerConsumer.Models.Model;

namespace ProducerConsumer.WebAPI.Controllers
{
    public class ConsumerController : ApiBaseController
    {
        private readonly IConsumerService _consumerService;

        public ConsumerController(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsumeMessage()
        {
            var consumeMessageResponse = await _consumerService.ConsumeMessage();

            if (consumeMessageResponse.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(new ResponseWrapper<ConsumeMessageResponse>()
                {
                    StatusCode = consumeMessageResponse.StatusCode,
                    Status = consumeMessageResponse.Status,
                    Message = consumeMessageResponse.Message,
                    Result = new ConsumeMessageResponse()
                    {
                        SuccessCount = consumeMessageResponse.SuccessCount,
                        ErrorCount = consumeMessageResponse.ErrorCount
                    }
                });
            }
            
            return StatusCode(consumeMessageResponse.StatusCode, new ErrorDetail()
            {
                StatusCode = consumeMessageResponse.StatusCode,
                Status = consumeMessageResponse.Status,
                Message = consumeMessageResponse.Message
            });
        }
    }
}
