using System.Threading.Tasks;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Account
{
    public interface IAccountService
    {
        Task<AccountModel> GetAccountByChatIdAsync(long? chatId);

        Task CreateAsync(AccountModel account);

        Task UpdateAsync(AccountModel account);
    }
}
