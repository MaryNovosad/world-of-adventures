using System.Collections.Generic;

namespace WorldOfAdventures.Models
{
    public class UserAdventureStep : AdventureStep
    {
        public UserAdventureStep()
        {

        }

        public UserAdventureStep(string sentence, ICollection<AdventureStep> nextSteps, string? answer) : base(sentence, nextSteps, answer)
        {
        }

        public bool IsChosen { get; set; }
    }
}