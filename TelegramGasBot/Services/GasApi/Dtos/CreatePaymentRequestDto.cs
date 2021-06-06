namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class CreatePaymentRequestDto
    {
        public string PersonalAccountNumber { get; set; }

        public PaymentDto Payment { get; set; }
    }
}
