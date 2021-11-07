using System.Collections.Generic;

namespace WorldOfAdventures.Models
{
    public class UserAdventureStep
    {
        public UserAdventureStep(string sentence, ICollection<UserAdventureStep>? nextSteps = null, string? answer = null)
        {
            Sentence = sentence;
            NextSteps = nextSteps;
            Answer = answer;
        }

        public string? Answer { get; set; }
        public string Sentence { get; set; }
        public ICollection<UserAdventureStep>? NextSteps { get; set; }
        public bool? IsChosen { get; set; }
    }
}