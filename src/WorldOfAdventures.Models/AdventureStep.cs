using System.Collections.Generic;

namespace WorldOfAdventures.Models
{
    public class AdventureStep
    {
        public AdventureStep()
        {
        }

        public AdventureStep(string sentence, ICollection<AdventureStep>? nextSteps, string? answer)
        {
            Sentence = sentence;
            NextSteps = nextSteps;
            Answer = answer;
        }

        public string? Answer { get; set; }
        public string Sentence { get; set; }
        public ICollection<AdventureStep>? NextSteps { get; set; }
    }
}