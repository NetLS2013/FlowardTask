using MediatorService.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SharedDto.DtoModels.ResponseDtoModels;
using SharedDto.Models;
using System.Text;
using System.Text.Json;

namespace MediatorService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmailController : ControllerBase
    {
        private IRabbitMQService RabbitMQ { get; }

        public EmailController(IRabbitMQService rabbitMQService)
        {
            RabbitMQ = rabbitMQService;
        }

        [HttpPost]
        public string Send(EmailDto email)
        {
            BaseResponse response = new BaseResponse();

            if (ModelState.IsValid)
            {
                response.IsSuccess = RabbitMQ.SendEmail(email.Body);
            }

            return JsonSerializer.Serialize(response);
        }
    }
}
