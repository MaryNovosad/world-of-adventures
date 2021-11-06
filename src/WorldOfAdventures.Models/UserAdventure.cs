namespace WorldOfAdventures.Models
{
    public class UserAdventure
    {
        public UserAdventure(User user, string adventureName, UserAdventureStep userAdventureInitialStep)
        {
            User = user;
            AdventureName = adventureName;
            InitialStep = userAdventureInitialStep;
        }

        public string AdventureName { get; set; }
        public UserAdventureStep InitialStep { get; set; }
        public User User { get; set; }
    }
}
