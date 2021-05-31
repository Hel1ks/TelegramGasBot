using System;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class ReadingDto
    {
        public decimal Value { get; set; }

        public DateTime DateTime { get; set; }
    }
}