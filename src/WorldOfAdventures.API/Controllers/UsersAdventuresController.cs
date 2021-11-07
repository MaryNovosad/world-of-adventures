using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldOfAdventures.BusinessLogic;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UsersAdventuresController : ControllerBase
    {
        private readonly IAdventureService _adventureService;
        private readonly ILogger<AdventuresController> _logger;

        public UsersAdventuresController(IAdventureService adventureService, ILogger<AdventuresController> logger)
        {
            _adventureService = adventureService;
            _logger = logger;
        }

        [HttpGet("{userName}/adventures/{adventureName}")]
        public async Task<UserAdventure> Get(string userName, string adventureName)
        {
            return await _adventureService.FindAsync(userName, adventureName);
        }

        [HttpPut("{userName}/adventures/{adventureName}")]
        public async Task<IActionResult> ChooseAnswer(string userName, string adventureName, UserChoice userChoice)
        {
            await _adventureService.ChooseAnswerAsync(userName, adventureName, userChoice);

            return Ok();
        }
    }
}
