using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using WorldOfAdventures.DAL.Models;

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

        public async Task UpdateAsync(Adventure adventure)
        {
            var filter = Builders<Adventure>.Filter.Eq("Name", adventure.Name);
            var toUpdate = Builders<Adventure>.Update.Set("InitialStep", adventure.InitialStep);

            await _adventures.UpdateOneAsync(filter, toUpdate);
        }

        public async Task<Adventure?> FindAsync(string adventureName)
        {
            var filter = Builders<Adventure>.Filter.Eq("Name", adventureName);
            var adventures = await _adventures.FindAsync<Adventure>(filter);

            return adventures.SingleOrDefault();
        }
    }
}
