using System.ComponentModel.DataAnnotations;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.API.Requests
{
    public class CreateOrUpdateAdventureRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public AdventureStep InitialStep { get; set; }
    }
}
