using System;

namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; }
    }
}
