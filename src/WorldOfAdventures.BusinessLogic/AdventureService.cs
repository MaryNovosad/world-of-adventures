using System;
using System.Linq;
using System.Threading.Tasks;
using WorldOfAdventures.DAL;
using WorldOfAdventures.Models;
using Adventure = WorldOfAdventures.Models.Adventure;
using AdventureStep = WorldOfAdventures.Models.AdventureStep;

namespace WorldOfAdventures.BusinessLogic
{
    public class AdventureService : IAdventureService
    {
        private readonly IValidationService _validationService;
        private readonly IAdventureRepository _adventureRepository;
        private readonly IUserAdventureRepository _userAdventureRepository;

        public AdventureService(IValidationService validationService, IAdventureRepository adventureRepository, IUserAdventureRepository userAdventureRepository)
        {
            _validationService = validationService;
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

        public async Task<UserAdventure?> FindAsync(string userName, string adventureName)
        {
            var dbUserAdventure = await _userAdventureRepository.FindAsync(userName, adventureName);

            if (dbUserAdventure == null)
            {
                return null;
            }

            return await RestoreUserAdventureTreeAsync(dbUserAdventure);
        }

        public async Task ChooseAnswerAsync(string userName, string adventureName, UserChoice choice)
        {
            var userAdventureChoicesChain = await _userAdventureRepository.FindAsync(userName, adventureName);

            await _validationService.ValidateUserChoice(adventureName, await RestoreUserAdventureTreeAsync(userAdventureChoicesChain), choice);

            if (userAdventureChoicesChain == null)
            {
                var userAdventure = new DAL.Models.UserAdventure
                {
                    AdventureName = adventureName,
                    UserName = userName,
                    InitialChoice = new DAL.Models.UserChoice
                    {
                        Answer = choice.Answer
                    }
                };

                await _userAdventureRepository.CreateAsync(userAdventure);
            }
            else
            {
                AppendNewAnswer(userAdventureChoicesChain, choice.Answer);

                await _userAdventureRepository.UpdateAsync(userAdventureChoicesChain);
            }
        }

        // TODO: extract this logic to Factory which is able to construct adventure tree with highlighted user choices from adventure template and user choices chain
        private async Task<UserAdventure?> RestoreUserAdventureTreeAsync(DAL.Models.UserAdventure? userAdventureChoicesChain)
        {
            if (userAdventureChoicesChain == null)
            {
                return null;
            }

            var initialStep = (await FindAsync(userAdventureChoicesChain.AdventureName))?.InitialStep ?? throw new ArgumentException($"Adventure {userAdventureChoicesChain.AdventureName} does not exist");

            var userAdventureStep = MapUserAdventureStep(initialStep);
            var currentStep = userAdventureStep;

            var currentChoice = userAdventureChoicesChain.InitialChoice;

            do
            {
                Func<UserAdventureStep, bool> answersMatch = s => s.Answer == currentChoice.Answer;
                var chosenStep = currentStep.NextSteps?.SingleOrDefault(answersMatch);

                if (chosenStep == null)
                {
                    throw new Exception("User adventure data is inconsistent");
                }

                chosenStep.IsChosen = true;

                foreach (var step in currentStep.NextSteps.Where(s => !answersMatch(s)))
                {
                    step.IsChosen = false;
                }

                currentStep = chosenStep;
                currentChoice = currentChoice.NextChoice;
            }
            while (currentChoice != null);

            return new UserAdventure(userAdventureChoicesChain.UserName, userAdventureChoicesChain.AdventureName, userAdventureStep);
        }

        private void AppendNewAnswer(DAL.Models.UserAdventure userAdventureChoicesChain, string answer)
        {
            var lastChoice = userAdventureChoicesChain.InitialChoice;

            while (lastChoice.NextChoice != null)
            {
                lastChoice = lastChoice.NextChoice;
            }

            lastChoice.NextChoice = new DAL.Models.UserChoice
            {
                Answer = answer
            };
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

        private UserAdventureStep MapUserAdventureStep(AdventureStep adventureStep)
        {
            return new UserAdventureStep(adventureStep.Sentence, adventureStep.NextSteps?.Select(MapUserAdventureStep).ToList(), adventureStep.Answer);
        }
    }
}
