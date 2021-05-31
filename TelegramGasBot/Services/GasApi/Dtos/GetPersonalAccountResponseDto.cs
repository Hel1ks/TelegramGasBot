using System.Text.Json.Serialization;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class GetPersonalAccountResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseCodeEnum ResponseCode { get; set; }

        public string PersonalAccountNumber { get; set; }

        public string Address { get; set; }

        public string MeterNumber { get; set; }
    }
}
