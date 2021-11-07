using System.Threading.Tasks;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.BusinessLogic
{
    public interface IAdventureService
    {
        public Task CreateOrUpdateAsync(string adventureName, Adventure adventure);
        Task<Adventure?> FindAsync(string adventureName);
    }
}