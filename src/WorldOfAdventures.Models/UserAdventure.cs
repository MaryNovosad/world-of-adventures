namespace WorldOfAdventures.Models
{
    public class UserAdventure
    {
        public UserAdventure(string userName, string adventureName, UserAdventureStep userAdventureInitialStep)
        {
            UserName = userName;
            AdventureName = adventureName;
            InitialStep = userAdventureInitialStep;
        }

        public string AdventureName { get; set; }
        public UserAdventureStep InitialStep { get; set; }
        public string UserName { get; set; }
    }
}
