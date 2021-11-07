using System.Threading.Tasks;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.BusinessLogic
{
    public interface IAdventureService
    {
        public Task CreateOrUpdateAsync(string adventureName, Adventure adventure);
        Task<Adventure?> FindAsync(string adventureName);
        Task<UserAdventure?> FindAsync(string userName, string adventureName);
        Task ChooseAnswerAsync(string userName, string adventureName, UserChoice choice);
    }
}