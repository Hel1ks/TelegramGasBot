using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramGasBot.Services.Account;
using TelegramGasBot.Services.Account.Models;
using TelegramGasBot.Services.Command;
using TelegramGasBot.Services.Processing;
using TelegramGasBot.Services.Telegram;

namespace TelegramGasBot.Controllers
{
    [ApiController]
    [Route("")]
    public class TelegramBotController
    {
        private readonly ITelegramService telegramService;
        private readonly IAccountService accountService;
        private readonly ICommandService commandService;
        private readonly IProcessingService processingService;

        public TelegramBotController(
            ITelegramService telegramService,
            IAccountService accountService,
            ICommandService commandService,
            IProcessingService processingService)
        {
            this.telegramService = telegramService;
            this.accountService = accountService;
            this.commandService = commandService;
            this.processingService = processingService;
        }

        [HttpPost()]
        public async Task Update([FromBody] Update update)
        {
            var chatId = update?.Message?.Chat?.Id;
            var text = update?.Message?.Text;

            var accountDto = await this.accountService.GetAccountByChatIdAsync(chatId) ?? new AccountModel() { ChatId = chatId };

            var command = this.commandService.GetCommand(text, accountDto);

            var processCommandTelegramResponse = await this.processingService.ProcessCommandAsync(command, accountDto, text);

            this.telegramService.SendTelegramMessage(chatId, processCommandTelegramResponse.Message, processCommandTelegramResponse.MenuItems);
        }
    }
}