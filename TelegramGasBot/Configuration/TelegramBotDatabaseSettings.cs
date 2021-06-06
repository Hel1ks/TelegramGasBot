namespace TelegramGasBot.Configuration
{
    public class TelegramBotDatabaseSettings
    {
        public string AccountsCollectionName { get; set; }
        public string PaymentsCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
