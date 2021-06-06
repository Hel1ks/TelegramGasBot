using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramGasBot.Services.Command
{
    public interface ICommandService
    {
        Task<GetCommandDto> GetCommandAsync(Update request);
    }
}
