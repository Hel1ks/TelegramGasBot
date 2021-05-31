namespace TelegramGasBot.Services.GasApi.Dtos
{
    public class CreateReadingRequestDto
    {
        public string PersonalAccountNumber { get; set; }

        public ReadingDto Reading { get; set; }
    }
}
