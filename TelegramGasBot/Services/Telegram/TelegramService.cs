using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramGasBot.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private ITelegramBotClient telegramClient;

        public TelegramService(ITelegramBotClient telegramClient)
        {
            this.telegramClient = telegramClient;
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
    }
}
