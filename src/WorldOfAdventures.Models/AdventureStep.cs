using System.Collections.Generic;

namespace WorldOfAdventures.Models
{
    public class AdventureStep
    {
        public AdventureStep()
        {
        }

        public AdventureStep(string sentence, ICollection<AdventureStep> nextSteps, string? answer)
        {
            Sentence = sentence;
            NextSteps = nextSteps;
            Answer = answer;
        }

        public string? Answer { get; }
        public string Sentence { get; }
        public ICollection<AdventureStep> NextSteps { get; }
    }
}