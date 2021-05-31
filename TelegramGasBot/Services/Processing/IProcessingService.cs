using System.Threading.Tasks;
using TelegramGasBot.Enums;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Processing
{
    public interface IProcessingService
    {
        Task<ProcessCommandResponseDto> ProcessCommandAsync(UserCommandEnum commandEnum, AccountModel accountDto, string message);
    }
}
