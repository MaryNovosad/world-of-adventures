using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldOfAdventures.BusinessLogic;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.Api.Controllers
{
    [ApiController]
    [Route("v1/Adventure")]
    public class AdventureController : ControllerBase
    {
        private readonly IAdventureService _adventureService;
        private readonly ILogger<AdventureController> _logger;

        public AdventureController(IAdventureService adventureService, ILogger<AdventureController> logger)
        {
            _adventureService = adventureService;
            _logger = logger;
        }

        [HttpGet]
        public Adventure Get(string name)
        {
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Adventure adventure)
        {
            await _adventureService.CreateAdventureAsync(adventure);

            return Ok();
        }

        // TODO: update
        // TODO: delete

    }
}
