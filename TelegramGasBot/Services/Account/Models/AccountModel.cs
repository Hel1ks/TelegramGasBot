using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TelegramGasBot.Services.Account.Models
{
    public class AccountModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public long? ChatId { get; set; }

        public StateModel State { get; set; }

        public IEnumerable<PersonalAccountModel> PersonalAccounts { get; set; }
    }
}
