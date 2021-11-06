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

        public Adventure()
        {

        }

        public AdventureStep InitialStep { get; set; }
    }
}
