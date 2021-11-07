using System.Collections.Generic;
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

    public class AdventureStep
    {
        public string? Answer { get; set; }
        public string Sentence { get; set; }
        public ICollection<AdventureStep>? NextSteps { get; set; }
    }
}