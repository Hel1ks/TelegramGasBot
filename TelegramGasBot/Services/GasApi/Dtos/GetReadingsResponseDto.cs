using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class GetReadingsResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseCodeEnum ResponseCode { get; set; }

        public IEnumerable<ReadingDto> Readings { get; set; }
    }
}
