using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelegramGasBot.Services.Payment
{
    public class PaymentModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PersonalAccountNumber { get; set; }
        
        public decimal Amount { get; set; }
    }
}
