namespace WorldOfAdventures.DAL.Models
{
    public class UserChoice
    {
        public string Answer { get; set; }
        public UserChoice? NextChoice { get; set; }
    }
}