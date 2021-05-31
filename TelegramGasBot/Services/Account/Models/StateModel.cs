using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TelegramGasBot.Enums;

namespace TelegramGasBot.Services.Account.Models
{
    public class StateModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]         
        public UserStateEnum StateEnum { get; set; }

        public string StateItemValue { get; set; }
    }
}
