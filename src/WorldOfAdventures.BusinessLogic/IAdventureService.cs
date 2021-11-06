using System.Threading.Tasks;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.BusinessLogic
{
    public interface IAdventureService
    {
        public Task CreateAdventureAsync(Adventure adventure);
    }
}