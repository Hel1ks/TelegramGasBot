using System.Threading.Tasks;
using TelegramGasBot.Services.Command;

namespace TelegramGasBot.Services.Processing
{
    public interface IProcessingService
    {
        Task ProcessCommandAsync(GetCommandDto dto);
    }
}
