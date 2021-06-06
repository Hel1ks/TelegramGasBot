using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramGasBot.Services.Command;
using TelegramGasBot.Services.Processing;

namespace TelegramGasBot.Controllers
{
    [ApiController]
    [Route("")]
    public class TelegramBotController
    {
        private readonly ICommandService commandService;
        private readonly IProcessingService processingService;

        public TelegramBotController(
            ICommandService commandService,
            IProcessingService processingService)
        {
            this.commandService = commandService;
            this.processingService = processingService;
        }

        [HttpPost()]
        public async Task Update([FromBody] Update request)
        {
            var commandDto = await this.commandService.GetCommandAsync(request);

            await this.processingService.ProcessCommandAsync(commandDto);
        }
    }
}