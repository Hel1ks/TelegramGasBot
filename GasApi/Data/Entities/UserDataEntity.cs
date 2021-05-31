using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GasApi.Data.Entities
{
    public class UserDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PersonalAccountNumber { get; set; }

        public string Address { get; set; }

        public string MeterNumber { get; set; }

        public IEnumerable<ReadingEntity> Readings{ get; set; }

        public IEnumerable<PaymentEntity> Payments { get; set; }
    }
}
