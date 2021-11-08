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
            var filter = BuildFilterDefinitionBy(userName, adventureName);

            return (await _usersAdventures.FindAsync<UserAdventure>(filter)).SingleOrDefault();
        }

        public async Task<ICollection<UserAdventure>> FindAsync(string adventureName)
        {
            var filter = Builders<UserAdventure>.Filter.Eq("AdventureName", adventureName);

            return (await _usersAdventures.FindAsync<UserAdventure>(filter)).ToList();
        }

        public async Task CreateAsync(UserAdventure userAdventureChoicesChain)
        {
            if (await FindAsync(userAdventureChoicesChain.UserName, userAdventureChoicesChain.AdventureName) != null)
            {
                throw new ArgumentException($"User {userAdventureChoicesChain.UserName} has already taken the adventure {userAdventureChoicesChain.AdventureName}");
            }

            await _usersAdventures.InsertOneAsync(userAdventureChoicesChain);
        }

        public async Task UpdateAsync(UserAdventure userAdventureChoicesChain)
        {
            var filter = BuildFilterDefinitionBy(userAdventureChoicesChain.UserName, userAdventureChoicesChain.AdventureName);
            var toUpdate = Builders<UserAdventure>.Update.Set("InitialChoice", userAdventureChoicesChain.InitialChoice);

            await _usersAdventures.UpdateOneAsync(filter, toUpdate);
        }

        private FilterDefinition<UserAdventure> BuildFilterDefinitionBy(string userName, string adventureName)
        {
            var adventureNameFilterDefinition = Builders<UserAdventure>.Filter.Eq("AdventureName", adventureName);
            var userNameFilterDefinition = Builders<UserAdventure>.Filter.Eq("UserName", userName);

            return Builders<UserAdventure>.Filter.And(adventureNameFilterDefinition, userNameFilterDefinition);
        }
    }
}
