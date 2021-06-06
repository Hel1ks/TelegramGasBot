using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGasBot.Configuration;

namespace TelegramGasBot.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private ITelegramBotClient telegramClient;

        private TelegramBotSettings botSettings;

        private const string currency = "UAH";

        public TelegramService(ITelegramBotClient telegramClient, TelegramBotSettings botSettings)
        {
            this.telegramClient = telegramClient;
            this.botSettings = botSettings;
        }

        public void SendTelegramMessage(long? chatId, string message, IEnumerable<string> menuItems)
        {
            var keyboard = new List<IEnumerable<KeyboardButton>>();

            foreach (var menuItem in menuItems)
            {
                keyboard.Add(new[]
                {
                    new KeyboardButton(menuItem)
                });
            }

            var replyMarkup = new ReplyKeyboardMarkup(keyboard, true);

            if (message != null && chatId != null)
            {
                telegramClient.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup);
            }
        }

        public void SendTelegramPayment(long? chatId, string amount, string paymendId)
        {
            var title = "Оплата";
            var description = "Оплата";

            var amountValue = (int)(decimal.Parse(amount, CultureInfo.InvariantCulture) * 100);

            var prices = new[] { new LabeledPrice() { Label = "Оплата", Amount = amountValue } };

            telegramClient.SendInvoiceAsync((int)chatId, title, description, paymendId, this.botSettings.PaymentProviderToken, "", currency, prices);
        }

        public void ConfirmTelegramPayment(string queryId)
        {
            telegramClient.AnswerPreCheckoutQueryAsync(queryId);
        }
    }
}
