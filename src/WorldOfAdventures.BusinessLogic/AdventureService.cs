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

        public async Task<UserAdventure?> FindAsync(string userName, string adventureName)
        {
            var dbUserAdventure = await _userAdventureRepository.FindAsync(userName, adventureName);

            if (dbUserAdventure == null)
            {
                return null;
            }

            var adventureStep = (await FindAsync(adventureName))?.InitialStep ?? throw new ArgumentException($"Adventure {adventureName} does not exist");

            var userAdventureStep = MapUserAdventureStep(adventureStep);
            var currentStep = userAdventureStep; 

            var currentChoice = dbUserAdventure.InitialChoice;

            // null checks

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

            return new UserAdventure(userName, adventureName, userAdventureStep);
        }

        public async Task ChooseAnswerAsync(string userName, string adventureName, UserChoice choice)
        {
            var existingUserAdventure = await _userAdventureRepository.FindAsync(userName, adventureName);

            await ValidateUserChoiceAsync(adventureName, existingUserAdventure, choice);

            if (existingUserAdventure == null)
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
                AppendNewAnswer(existingUserAdventure, choice.Answer);

                await _userAdventureRepository.UpdateAsync(existingUserAdventure);
            }
        }

        private void AppendNewAnswer(DAL.Models.UserAdventure existingUserAdventure, string answer)
        {
            var lastChoice = existingUserAdventure.InitialChoice;

            while (lastChoice.NextChoice != null)
            {
                lastChoice = lastChoice.NextChoice;
            }

            lastChoice.NextChoice = new DAL.Models.UserChoice
            {
                Answer = answer
            };
        }

        private async Task ValidateUserChoiceAsync(string adventureName, DAL.Models.UserAdventure? existingUserAdventure, UserChoice choice)
        {
            var adventureTemplate = await FindAsync(adventureName);

            if (adventureTemplate == null)
            {
                throw new ArgumentException($"Adventure {adventureName} does not exist");
            }

            var currentUserChoice = existingUserAdventure?.InitialChoice;
            var isInitialChoice = choice.AdventureLevel == 1;

            var currentAdventureStep = adventureTemplate.InitialStep.NextSteps.SingleOrDefault(s => s.Answer ==
                (currentUserChoice?.Answer ??
                 (isInitialChoice
                     ? choice.Answer
                     : throw new ArgumentException("User hasn't passed the earlier steps of his adventure journey yet"))));

            for (int i = 1; i < choice.AdventureLevel; i++)
            {
                if (currentUserChoice == null)
                {
                    throw new ArgumentException("User hasn't passed the earlier steps of his adventure journey yet");
                }

                currentUserChoice = currentUserChoice.NextChoice;
                var answerToCompare = i == choice.AdventureLevel - 1 
                    ? choice.Answer
                    : currentUserChoice?.Answer ?? throw new ArgumentException("User hasn't passed the earlier steps of his adventure journey yet");

                currentAdventureStep = currentAdventureStep.NextSteps.SingleOrDefault(s => s.Answer == answerToCompare);
            }

            if (currentUserChoice != null)
            {
                throw new ArgumentException("User is already on a further step in his adventure journey");
            }

            if (currentAdventureStep == null)
            {
                throw new ArgumentException("It's not possible to choose such answer option on current adventure step");
            }
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
