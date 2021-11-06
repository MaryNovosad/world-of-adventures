using System;
using System.Threading.Tasks;
using WorldOfAdventures.DAL;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.BusinessLogic
{
    public class AdventureService : IAdventureService
    {
        private readonly IAdventureRepository _adventureRepository;

        public AdventureService(IAdventureRepository adventureRepository)
        {
            _adventureRepository = adventureRepository;
        }

        public async Task CreateAdventureAsync(Adventure adventure)
        {
            await _adventureRepository.CreateAsync(adventure);
        }
    }
}
