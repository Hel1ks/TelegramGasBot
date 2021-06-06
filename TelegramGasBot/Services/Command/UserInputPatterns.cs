using TelegramGasBot.Constants;

namespace TelegramGasBot.Services.Command
{
    public static class UserInputPatterns
    {
        public const string PersonalAccountPattern = @"^\d{9}$";
        public const string MeterReadingPattern = @"^\d{1,5}\.\d{2}$";
        public const string MeterNumberPattern = @"^\d{8}$";
        public const string PaymentAmountPattern = @"^\d{1,5}\.\d{2}$";
        public const string SendReadingsPattern = "^" + MenuItemsConstants.SendReadings + "$";
        public const string PayPattern = "^" + MenuItemsConstants.Pay + "$";
        public const string DeleteSpecificPersonalAccountPattern = "^" + MenuItemsConstants.DeleteSpecificPersonalAccount + "$";
    }
}
