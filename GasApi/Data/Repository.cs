using GasApi.Data.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace GasApi.Data
{
    public class Repository : IRepository
    {
        private readonly IMongoCollection<UserDataEntity> entities;

        public Repository(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            entities = database.GetCollection<UserDataEntity>(settings.CollectionName);
        }

        public async Task<UserDataEntity> GetByPersonalAccountNumber(string personalAccountNumber)
        {
            return await entities.FindAsync<UserDataEntity>(d => d.PersonalAccountNumber == personalAccountNumber).GetAwaiter().GetResult().FirstOrDefaultAsync();
        }

        public async Task Create(UserDataEntity account)
        {
            await entities.InsertOneAsync(account);
        }

        public async Task Update(UserDataEntity account)
        {
            await entities.ReplaceOneAsync(a => a.PersonalAccountNumber == account.PersonalAccountNumber, account);
        }
    }
}
