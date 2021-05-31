using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class GetPaymentsResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseCodeEnum ResponseCode { get; set; }

        public IEnumerable<PaymentDto> Payments { get; set; }
    }
}
