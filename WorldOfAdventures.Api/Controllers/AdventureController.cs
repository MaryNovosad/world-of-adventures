using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WorldOfAdventures.Api.Controllers
{
    [ApiController]
    [Route("Adventure")]
    public class AdventureController : ControllerBase
    {
        private readonly ILogger<AdventureController> _logger;

        public AdventureController(ILogger<AdventureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Adventure Get()
        {
            return new Adventure();
        }
    }
}
