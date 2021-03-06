using System.Collections.Generic;

namespace WorldOfAdventures.Models
{
    public class AdventureStep
    {
        public string? Answer { get; set; }
        public string Sentence { get; set; }
        public ICollection<AdventureStep>? NextSteps { get; set; }
    }
}