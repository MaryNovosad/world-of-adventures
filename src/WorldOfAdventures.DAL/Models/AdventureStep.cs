using System.Collections.Generic;

namespace WorldOfAdventures.DAL.Models
{
    public class AdventureStep
    {
        public string? Answer { get; set; }
        public string Sentence { get; set; }
        public ICollection<AdventureStep>? NextSteps { get; set; }
    }
}