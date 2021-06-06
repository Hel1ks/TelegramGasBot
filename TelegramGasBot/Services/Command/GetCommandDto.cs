using TelegramGasBot.Enums;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Command
{
    public class GetCommandDto
    {
        public UserCommandEnum CommandEnum { get; set; }

        public AccountModel AccountDto { get; set; }

        public string Message { get; set; }
    }
}
