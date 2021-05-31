using GasApi.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("GetPersonalAccount")]
        public async Task<GetPersonalAccountResponse> GetPersonalAccount([FromBody] GetPersonalAccountRequest request) =>
            await this.mediator.Send(request);

        [HttpPost("CreateReading")]
        public async Task<CreateReadingResponse> CreateReading([FromBody] CreateReadingRequest request) =>
            await this.mediator.Send(request);

        [HttpPost("GetReadings")]
        public async Task<GetReadingsResponse> GetReadings([FromBody] GetReadingsRequest request) =>
            await this.mediator.Send(request);

        [HttpPost("GetPayments")]
        public async Task<GetPaymentsResponse> GetPayments([FromBody] GetPaymentsRequest request) =>
            await this.mediator.Send(request);
    }
}
