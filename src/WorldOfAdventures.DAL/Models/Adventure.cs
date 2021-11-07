using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorldOfAdventures.DAL.Models
{
    public class Adventure
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public AdventureStep InitialStep { get; set; }
    }
}