using System.Threading.Tasks;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.DAL
{
    public interface IAdventureRepository
    {
        Task<Adventure?> FindAsync(string adventureName);
        Task CreateAsync(Adventure adventure);
    }
}