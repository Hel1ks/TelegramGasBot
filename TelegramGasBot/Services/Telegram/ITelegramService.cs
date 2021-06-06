using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramGasBot.Services.Telegram
{
    public interface ITelegramService
    {
        void SendTelegramMessage(long? chatId, string message, IEnumerable<string> menuItems);

        void SendTelegramPayment(long? chatId, string amount, string paymendId);

        void ConfirmTelegramPayment(string queryId);
    }
}
