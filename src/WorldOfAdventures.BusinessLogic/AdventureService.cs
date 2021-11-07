using System;
using System.Linq;
using System.Threading.Tasks;
using WorldOfAdventures.DAL;
using Adventure = WorldOfAdventures.Models.Adventure;
using AdventureStep = WorldOfAdventures.Models.AdventureStep;

namespace WorldOfAdventures.BusinessLogic
{
    public class AdventureService : IAdventureService
    {
        private readonly IAdventureRepository _adventureRepository;
        private readonly IUserAdventureRepository _userAdventureRepository;

        public AdventureService(IAdventureRepository adventureRepository, IUserAdventureRepository userAdventureRepository)
        {
            _adventureRepository = adventureRepository;
            _userAdventureRepository = userAdventureRepository;
        }

        public async Task CreateOrUpdateAsync(string adventureName, Adventure adventure)
        {
            if (adventureName != adventure.Name)
            {
                throw new ArgumentException("Adventure names are not consistent");
            }

            if (await _adventureRepository.FindAsync(adventureName) != null)
            {
                var usersAdventures = await _userAdventureRepository.FindAsync(adventureName);

                if (usersAdventures.Any())
                {
                    throw new ArgumentException(
                        "It's not possible to alter an adventure template after some users have already taken it. Please create a new template instead.");
                }

                await _adventureRepository.UpdateAsync(MapAdventure(adventure));
            }
            else
            {
                await _adventureRepository.CreateAsync(MapAdventure(adventure));
            }
        }

        public async Task<Adventure?> FindAsync(string adventureName)
        {
            var dbAdventure = await _adventureRepository.FindAsync(adventureName);

            if (dbAdventure == null)
            {
                return null;
            }

            return MapAdventure(dbAdventure);
        }

        private DAL.Models.Adventure MapAdventure(Adventure adventure)
        {
            return new DAL.Models.Adventure
            {
                Name = adventure.Name,
                InitialStep = MapAdventureStep(adventure.InitialStep)
            };
        }

        private DAL.Models.AdventureStep MapAdventureStep(AdventureStep adventureStep)
        {
            return new DAL.Models.AdventureStep
            {
                Answer = adventureStep.Answer,
                Sentence = adventureStep.Sentence,
                NextSteps = adventureStep.NextSteps?.Select(MapAdventureStep).ToList()
            };
        }

        private Adventure MapAdventure(DAL.Models.Adventure adventure)
        {
            return new Adventure(adventure.Name, MapAdventureStep(adventure.InitialStep));
        }

        private AdventureStep MapAdventureStep(DAL.Models.AdventureStep adventureStep)
        {
            return new AdventureStep
            {
                Answer = adventureStep.Answer,
                Sentence = adventureStep.Sentence,
                NextSteps = adventureStep.NextSteps?.Select(MapAdventureStep).ToList()
            };
        }
    }
}
