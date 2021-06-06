using MongoDB.Driver;
using System.Threading.Tasks;
using TelegramGasBot.Configuration;
using TelegramGasBot.Services.Account.Models;

namespace TelegramGasBot.Services.Account
{
    public class AccountService: IAccountService
    {
        private readonly IMongoCollection<AccountModel> accounts;

        public AccountService(TelegramBotDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            accounts = database.GetCollection<AccountModel>(settings.AccountsCollectionName);
        }

        public async Task<AccountModel> GetAccountByChatIdAsync(long? chatId)
        {
            var account = await accounts.FindAsync(acc => acc.ChatId == chatId);

            return await account.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(AccountModel account)
        {
            await accounts.InsertOneAsync(account);
        }

        public async Task UpdateAsync(AccountModel account)
        {
            await accounts.ReplaceOneAsync(a => a.ChatId == account.ChatId, account);
        }
    }
}
