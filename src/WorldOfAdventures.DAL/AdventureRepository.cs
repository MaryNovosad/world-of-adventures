using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WorldOfAdventures.DAL.Models;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.DAL
{
    public class AdventureRepository : IAdventureRepository
    {
        private readonly IMongoCollection<Adventure> _adventures;

        public AdventureRepository(IMongoDatabase mongoDb)
        {
            _adventures = mongoDb.GetCollection<Adventure>("Adventures");
        }

        public async Task CreateAsync(Adventure adventure)
        {
            if (await FindAsync(adventure.Name) != null)
            {
                throw new ArgumentException("Adventure with such name already exists");
            }

            await _adventures.InsertOneAsync(adventure);
        }

        public async Task<Adventure?> FindAsync(string adventureName)
        {
            var filter = Builders<Adventure>.Filter.Eq("name", adventureName);
            var adventure = await _adventures.FindAsync<Adventure>(filter);

            return adventure.SingleOrDefault();
        }
    }
}
