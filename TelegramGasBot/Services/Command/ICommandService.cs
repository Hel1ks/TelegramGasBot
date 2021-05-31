using TelegramGasBot.Enums;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Command
{
    public interface ICommandService
    {
        UserCommandEnum GetCommand(string message, AccountModel account);
    }
}
