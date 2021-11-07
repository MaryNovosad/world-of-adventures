using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using WorldOfAdventures.DAL.Models;

namespace WorldOfAdventures.DAL
{
    public class UserAdventureRepository : IUserAdventureRepository
    {
        private readonly IMongoCollection<UserAdventure> _usersAdventures;

        public UserAdventureRepository(IMongoDatabase mongoDb)
        {
            _usersAdventures = mongoDb.GetCollection<UserAdventure>("UsersAdventures");
        }

        public async Task<UserAdventure?> FindAsync(string userName, string adventureName)
        {
            var adventureNameFilterDefinition = Builders<UserAdventure>.Filter.Eq("AdventureName", adventureName);
            var userNameFilterDefinition = Builders<UserAdventure>.Filter.Eq("UserName", userName);

            var unitedFilter = Builders<UserAdventure>.Filter.And(adventureNameFilterDefinition, userNameFilterDefinition);

            return (await _usersAdventures.FindAsync<UserAdventure>(unitedFilter)).SingleOrDefault();
        }

        public async Task<ICollection<UserAdventure>> FindAsync(string adventureName)
        {
            var filter = Builders<UserAdventure>.Filter.Eq("AdventureName", adventureName);

            return (await _usersAdventures.FindAsync<UserAdventure>(filter)).ToList();
        }

        public Task CreateAsync(UserAdventure userAdventure)
        {
            throw new NotImplementedException();
        }
    }
}
