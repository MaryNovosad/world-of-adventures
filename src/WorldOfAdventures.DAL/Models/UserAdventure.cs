using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorldOfAdventures.DAL.Models
{
    public class UserAdventure
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string AdventureName { get; set; }
        public UserChoice InitialChoice { get; set; }
    }
}