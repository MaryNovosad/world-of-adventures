using System.Collections.Generic;
using System.Threading.Tasks;
using WorldOfAdventures.DAL.Models;

namespace WorldOfAdventures.DAL
{
    public interface IUserAdventureRepository
    {
        Task<UserAdventure?> FindAsync(string userName, string adventureName);
        Task<ICollection<UserAdventure>> FindAsync(string adventureName);
        Task CreateAsync(UserAdventure userAdventure);
    }
}
