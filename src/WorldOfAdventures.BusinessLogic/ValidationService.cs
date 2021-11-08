using System;
using System.Linq;
using System.Threading.Tasks;
using WorldOfAdventures.DAL;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.BusinessLogic
{
    // TODO: refactor validation logic to use separate validation rule for each verification step, then use chain of responsibility pattern to validate all the steps. 
    // TODO: use validation result object which aggregates all validation rules instead of using exceptions which is less flexible way of validation.
    public class ValidationService: IValidationService
    {
        private readonly IAdventureRepository _adventureRepository;

        public ValidationService(IAdventureRepository adventureRepository)
        {
            _adventureRepository = adventureRepository;
        }

        public async Task ValidateUserChoice(string adventureName, UserAdventure? existingUserAdventure, UserChoice choice)
        {
            var adventureTemplate = await _adventureRepository.FindAsync(adventureName);

            if (adventureTemplate == null)
            {
                throw new ArgumentException($"Adventure {adventureName} does not exist");
            }

            var currentUserChoice = existingUserAdventure?.InitialStep.NextSteps.FirstOrDefault(s => s.IsChosen == true);
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

                currentUserChoice = currentUserChoice.NextSteps?.FirstOrDefault(s => s.IsChosen == true);
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
    }
}
