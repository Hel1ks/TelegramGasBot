using TelegramGasBot.Constants;

namespace TelegramGasBot.Services.Command
{
    public static class UserInputPatterns
    {
        public const string PersonalAccountPattern = @"^\d{9}$";
        public const string MeterReadingPattern = @"^\d{1,5}\.\d{2}$";
        public const string SendReadingsPattern = "^" + MenuItemsConstants.SendReadings + "$";
        public const string DeleteSpecificPersonalAccountPattern = "^" + MenuItemsConstants.DeleteSpecificPersonalAccount + "$";
    }
}
