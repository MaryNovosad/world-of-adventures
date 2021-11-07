namespace WorldOfAdventures.Models
{
    public class Adventure
    {
        public string Name { get; set; }
        public Adventure(string name, AdventureStep initialStep)
        {
            Name = name;
            InitialStep = initialStep;
        }

        public AdventureStep InitialStep { get; set; }
    }
}
