using System.Text.Json.Serialization;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class CreateReadingResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseCodeEnum ResponseCode { get; set; }
    }
}
