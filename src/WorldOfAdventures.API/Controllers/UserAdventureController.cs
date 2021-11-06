using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.Api.Controllers
{
    [Route("v1/UserAdventure")]
    [ApiController]
    public class UserAdventureController : ControllerBase
    {
        private readonly ILogger<AdventureController> _logger;

        public UserAdventureController(ILogger<AdventureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public UserAdventure Get(string userName, string adventureName)
        {
            return null;
        }

        [HttpPost]
        public IActionResult Take(UserAdventure userAdventureDto)
        {
            return Ok();
        }
    }
}
