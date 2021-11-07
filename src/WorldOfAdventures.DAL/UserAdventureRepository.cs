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

        public async Task CreateAsync(UserAdventure userAdventure)
        {
            if (await FindAsync(userAdventure.UserName, userAdventure.AdventureName) != null)
            {
                throw new ArgumentException($"User {userAdventure.UserName} has already taken the adventure {userAdventure.AdventureName}");
            }

            await _usersAdventures.InsertOneAsync(userAdventure);
        }

        public async Task UpdateAsync(UserAdventure userAdventure)
        {
            var filter = BuildFilterDefinitionBy(userAdventure.UserName, userAdventure.AdventureName);
            var toUpdate = Builders<UserAdventure>.Update.Set("InitialChoice", userAdventure.InitialChoice);

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
