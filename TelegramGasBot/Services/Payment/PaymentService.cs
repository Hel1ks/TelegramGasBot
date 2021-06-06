using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using TelegramGasBot.Configuration;

namespace TelegramGasBot.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<PaymentModel> accounts;

        public PaymentService(TelegramBotDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            accounts = database.GetCollection<PaymentModel>(settings.PaymentsCollectionName);
        }

        public async Task<PaymentModel> GetPaymentByIdAsync(string id)
        {
            var account = await accounts.FindAsync(p => p.Id == id);

            return await account.FirstOrDefaultAsync();
        }

        public async Task<string> CreateAsync(PaymentModel payment)
        {
            payment.Id = ObjectId.GenerateNewId().ToString();

            await accounts.InsertOneAsync(payment);

            return payment.Id;
        }

        public async Task UpdateAsync(PaymentModel payment)
        {
            await accounts.ReplaceOneAsync(a => a.Id == payment.Id, payment);
        }
    }
}
