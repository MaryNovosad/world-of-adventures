using System.Threading.Tasks;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.BusinessLogic
{
    public interface IValidationService
    {
        Task ValidateUserChoice(string adventureName, UserAdventure? existingUserAdventure, UserChoice choice);
    }
}
