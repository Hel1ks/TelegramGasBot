using System.Text.Json.Serialization;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class CreatePaymentResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseCodeEnum ResponseCode { get; set; }
    }
}
